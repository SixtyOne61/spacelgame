using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using Tool;

[System.Serializable]
public class CompSpecialist : ComponentBase
{
	private GameObject _currentShield = null;
	
    public override void Update()
    {
        base.Update();

        // x button
        bool shieldInput = Input.GetKeyDown("joystick button 2");
        if (shieldInput	&& _currentShield == null)
        {
        	_currentShield = Builder.Instance.Build(Builder.FactoryType.Gameplay, (int)BuilderGameplay.Type.Shield, Vector3.zero, Quaternion.identity, Owner.transform);
        	// set Shield
        	EntShield ent = _currentShield.GetComponent<EntShield>();
        	EntPlayer entPlayer = Owner.GetComponent<EntPlayet>();
        	if(ent != null && entPlayer != null)
        	{
        		ent.X = entPlayer.X;
        		ent.Y = entPlayer.Y;
        		ent.Z = entPlayer.Z;
        	}
        }
    }
}
    