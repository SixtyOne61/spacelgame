using Engine;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EntSpeedFx : SpacelEntity
{
	// ref to player entity
	private EntPlayer _player = null;
	
	[Tooltip("Particle system associate to this entity")]
	public ParticleSystem Particle;
	
	public override void Start()
	{
		base.Start();
		
		_player = GetComponentInParent<EntPlayer>();
		if(_player == null)
		{
			Debug.LogError("Player wasn't found.");
		}
	}
	
	public override void Update()
	{
		base.Update();
		
		float ratio = _player.ComponentController.Ratio;
		
	}
}
