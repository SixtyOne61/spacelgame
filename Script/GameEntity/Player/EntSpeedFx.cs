using Engine;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EntSpeedFx : SpacelEntity
{
	// ref to player entity
	private EntPlayer _player = null;
	
	// Particle system associate to this entity
	private ParticleSystem _particle = null;
	
	// life time base
	float _startLifeTime = 0.0f;
	
	public override void Start()
	{
		base.Start();
		
		_player = GetComponentInParent<EntPlayer>();
		if(_player == null)
		{
			Debug.LogError("Player wasn't found.");
		}
		
		_particle = GetComponent<ParticleSystem>();
		if(_particle == null)
		{
			Debug.LogError("Particle system not found.");
		}
		
		_startLifeTime = _particle.main.startLifetime;
	}
	
	public override void Update()
	{
		base.Update();
		
		float ratio = _player.ComponentController.Ratio;
		_particle.main.startLifetime = ratio * _startLifeTime;
	}
}
    