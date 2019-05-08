using UnityEngine;
using System.Collections.Generic;
using Engine;

[RequireComponent(typeof(ParticleSystem))]
public class EntLoot : SpacelEntity
{
    [Tooltip("Max distance between particle")]
    public float MaxDistance = 1.0f;

    public int MaxConnections = 3;
    public int MaxLineRenderers = 50;

    // param
    public Tool.SCROneValue Param;

    // the particle system of this loot
    private ParticleSystem _system = null;
    private ParticleSystem.Particle[] _particles;
    private ParticleSystem.MainModule _mainModule;

    public LineRenderer LineRendererTemplate;
    private List<LineRenderer> _lineRenderers = new List<LineRenderer>();

    [HideInInspector]
    public float Ressource = 0.0f;
    private float _ressourceGiven = 0.0f;

    private Transform _transform;

    // nb particle per second
    private float _rateOverTime = 0.0f;
    // true if attrack mode
    private bool _isAttrackMode = false;
    
    public override void Start()
    {
        base.Start();
        _system = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_system.main.maxParticles];
        _mainModule = _system.main;

        _transform = transform;

        _rateOverTime = _system.emission.rateOverTime.constantMax;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // first we need to check if we have player near our system
        foreach(EntPlayer entPlayer in GameManager.Instance.Players)
        {
            // TO DO : no magic number
            if (Vector3.Distance(entPlayer.Camera.transform.position, transform.position) < 10.0f)
            {
                if(!_isAttrackMode)
                {
                    _isAttrackMode = true;
                    ParticleSystem.EmissionModule em = _system.emission;
                    ParticleSystem.MinMaxCurve rateOverTimeCurve = em.rateOverTime;
                    rateOverTimeCurve.constantMax = _rateOverTime * 4;
                }

                Attrack(entPlayer.Camera.transform.position);

                // give ressource to target (one second for give all)
                float ressource = Ressource * (Time.fixedDeltaTime / Param.Value);
                entPlayer.Ressource += ressource;
                _ressourceGiven += ressource;

                // we have given all ressource, destroy loot
                if (_ressourceGiven >= Ressource)
                {
                    Tool.Builder.Instance.DestroyGameObject(gameObject, false);
                }
                break;
            }
            else if (_isAttrackMode)
            {
                _isAttrackMode = false;
                ParticleSystem.EmissionModule em = _system.emission;
                ParticleSystem.MinMaxCurve rateOverTimeCurve = em.rateOverTime;
                rateOverTimeCurve.constantMax = _rateOverTime;
            }
        }
    }

    public void LateUpdate()
    {
        int maxParticles = _mainModule.maxParticles;

        if(_system == null || _particles.Length < maxParticles)
        {
            _particles = new ParticleSystem.Particle[maxParticles];
        }

        int lrIdx = 0;
        int lineRendererCount = _lineRenderers.Count;

        if(lineRendererCount > MaxLineRenderers)
        {
            for(int i = MaxLineRenderers; i < lineRendererCount; i++)
            {
                Destroy(_lineRenderers[i].gameObject);
            }
            int removeCount = lineRendererCount - MaxLineRenderers;
            _lineRenderers.RemoveRange(MaxLineRenderers, removeCount);
            lineRendererCount -= removeCount;
        }

        if (MaxConnections > 0 && MaxLineRenderers > 0)
        {
            _system.GetParticles(_particles);
            int particleCount = _system.particleCount;

            float maxDistanceSqr = MaxDistance * MaxDistance;

            ParticleSystemSimulationSpace simulationSpace = _mainModule.simulationSpace;

            switch (simulationSpace)
            {
                case ParticleSystemSimulationSpace.Local:
                    {
                        _transform = transform;
                        break;
                    }

                case ParticleSystemSimulationSpace.Custom:
                    {
                        _transform = _mainModule.customSimulationSpace;
                        break;
                    }

                case ParticleSystemSimulationSpace.World:
                    {
                        _transform = transform;
                        break;
                    }

                default:
                    Debug.LogError("Simulation space doesn't manage.");
                    break;
            }


            for (int i = 0; i < particleCount; ++i)
            {
                if (lrIdx == MaxLineRenderers)
                {
                    break;
                }

                Vector3 p1 = _particles[i].position;

                int connections = 0;

                for (int j = i + 1; j < particleCount; ++j)
                {
                    Vector3 p2 = _particles[j].position;
                    float distanceSqr = Vector3.Magnitude(p1 - p2);

                    if (distanceSqr <= maxDistanceSqr)
                    {
                        LineRenderer lr;

                        if (lrIdx == lineRendererCount)
                        {
                            lr = Instantiate(LineRendererTemplate, _transform, false);
                            _lineRenderers.Add(lr);

                            lineRendererCount++;
                        }

                        lr = _lineRenderers[lrIdx];

                        lr.enabled = true;
                        lr.useWorldSpace = simulationSpace == ParticleSystemSimulationSpace.World;

                        lr.SetPosition(0, p1);
                        lr.SetPosition(1, p2);

                        lrIdx++;
                        connections++;

                        if(connections >= MaxConnections)
                        {
                            break;
                        }
                    }
                }
            }
        }

        for(int i = lrIdx; i < _lineRenderers.Count; ++i)
        {
            _lineRenderers[i].enabled = false;
        }
    }

    private void Attrack(Vector3 target)
    {
        int nbParticleAlive = _system.GetParticles(_particles);
        float percent = 0.0f;
        for (int i = 0; i < nbParticleAlive; ++i)
        {
            percent = _particles[i].startLifetime - _particles[i].remainingLifetime;
            // set position
            _particles[i].position = _system.transform.InverseTransformPoint(Vector3.Lerp(_system.transform.TransformPoint(_particles[i].position), target, percent));
            // set color
        }

        _system.SetParticles(_particles, nbParticleAlive);
    }
}
