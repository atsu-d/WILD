using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroneMotor : MonoBehaviour, IMotor
{
    [SerializeField] private float maxPower = 4f;
    private float numberOfMotors, gravity;

    public void OnInitializeMotor()
    {
        gravity = Physics.gravity.magnitude;
        numberOfMotors = transform.parent.childCount;
    }

    public void OnUpdateMotor(Rigidbody _rb, DroneController _inputs)
    {
        Vector3 _upwardVector = transform.up;
        _upwardVector.x = 0;
        _upwardVector.z = 0;
        float _upwardForce = gravity * (1 - _upwardVector.magnitude);
        

        Vector3 _motorForce = Vector3.zero;
        _motorForce = transform.up * ((_rb.mass * gravity + _upwardForce) + (_inputs.Throttle * maxPower)) / numberOfMotors;

        _rb.AddForce(_motorForce, ForceMode.Force);
    }
}
