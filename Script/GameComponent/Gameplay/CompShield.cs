using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompShield : ComponentBase
{
	public override void Update()
	{
		float value = Input.GetAxis("");
		base.Update();
	}
}
