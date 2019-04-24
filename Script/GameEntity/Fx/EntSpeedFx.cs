using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntSpeedFx : EntTrailRatio
{
	private CompController _refController = null;
    
    public override void Start()
    {
        base.Start();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(_refController != null)
        {
        	Ratio = _refController.Ratio;
        }
    }
    
    public void SetRef(ref CompController comp)
    {
    	_refController = comp;
    }
}
