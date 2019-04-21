using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;

[System.Serializable]
public class CompController : ComponentBase
{
    public Tool.SCRController Param;

    private float _currentAcc = 0.0f;
    private float _currentSpeed = 0.0f;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // update speed
        UpdateSpeed();

        // update rotation of ship
        UpdateRotation();
    }

    private void UpdateSpeed()
    {
        // can be negative
        float valAcc = Input.GetAxis("VerticalRight");

        // reset acc if we start a new one
        if (valAcc == 0.0f && _currentAcc != 0.0f)
        {
            _currentAcc = 0.0f;
        }
        else
        {
            // update relative acc
            _currentAcc = Mathf.Clamp(_currentAcc + valAcc * Time.deltaTime, -1.0f, 1.0f);
        }

        // new acc, directed by curve
        float acc = Param.AccCurve.Evaluate(Mathf.Abs(_currentAcc));

        // if we are braking
        if (valAcc < 0.0f)
        {
            acc *= -1.0f;
        }

        // update relative speed
        _currentSpeed = Mathf.Clamp(_currentSpeed + acc * Param.Acc * Time.deltaTime, 0.0f, 1.0f);

        // new speed, directed by curve
        float speed = Param.SpeedCurve.Evaluate(_currentSpeed);
        Owner.transform.position += Owner.transform.forward * speed * Param.Speed;
    }

    private void UpdateRotation()
    {
        Owner.transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * Param.AngularSpeed);
        Owner.transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * Time.deltaTime * Param.AngularSpeed);
        Owner.transform.Rotate(Vector3.forward * Input.GetAxis("HorizontalRight") * Time.deltaTime * Param.AngularSpeed);
    }
}
