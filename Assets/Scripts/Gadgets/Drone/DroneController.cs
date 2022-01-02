using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DroneController : MonoBehaviour
{
    private Vector2 cyclic, pedal;
    private float throttle;

    public Vector2 Cyclic { get => cyclic; }
    public Vector2 Pedal { get => pedal; }
    public float Throttle { get => throttle; }

    private void OnCyclic(InputValue value)
    {
        cyclic = value.Get<Vector2>();
    }

    private void OnPedal(InputValue value)
    {
        pedal = value.Get<Vector2>();
    }

    private void OnThrottle(InputValue value)
    {
        throttle = value.Get<float>();
    }
}
