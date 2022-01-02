using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMotor
{
    void OnInitializeMotor();
    void OnUpdateMotor(Rigidbody _rb, DroneController _inputs);
}
