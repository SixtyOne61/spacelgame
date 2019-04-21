using UnityEngine;
using Engine;

[RequireComponent(typeof(ParticleSystem))]
public class EntLoot : SpacelEntity
{
    // param
    public Tool.SCROneValue Param;

    // the particle system of this loot
    private ParticleSystem _system = null;
    private ParticleSystem.Particle[] _particles;

    [HideInInspector]
    public float Ressource = 0.0f;
    private float _ressourceGiven = 0.0f;
    
    public override void Start()
    {
        base.Start();
        _system = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_system.main.maxParticles];
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // first we need to check if we have player near our system
        foreach(EntPlayer entPlayer in GameManager.Instance.Players)
        {
            if (Vector3.Distance(entPlayer.Camera.transform.position, transform.position) < 10.0f)
            {
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
