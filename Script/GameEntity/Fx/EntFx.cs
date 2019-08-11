using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[RequireComponent(typeof(ParticleSystem))]
public class EntFx : SpacelEntity
{
    // our particle system
    protected ParticleSystem _particleSystem;

    public override void Start()
    {
        base.Start();
        _particleSystem = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(!_particleSystem.isPlaying)
        {
            Tool.Builder.Instance.DestroyGameObject(gameObject, true);
        }
    }
}
