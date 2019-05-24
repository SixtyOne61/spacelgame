using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompTakeOnMe : ComponentBase
{
	[HideInInspector]
	public GameObject TriggerObj;
	
	public override void Update()
	{
		base.Update();

        // x button // to do change input
        bool takeOnMeInput = Input.GetKeyDown("joystick button 2");
        if(takeOnMeInput)
        {
        	TriggerObj.SetActive(TriggerObj.activeSelf);
        }
	}
}
    