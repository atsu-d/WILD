using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [SerializeField] private Camera droneCamera;
    public Camera playerCamera;

    private PlayerInput playerInput;
    private InputAction cyclicInput, pedalInput, throttleInput, exitInput;

    private Vector2 cyclic, pedal;
    private float throttle;

    public Vector2 Cyclic { get => cyclic; }
    public Vector2 Pedal { get => pedal; }
    public float Throttle { get => throttle; }

    private void Start()
    {
        if (droneCamera == null)
            droneCamera = GetComponentInChildren<Camera>();

        if (playerCamera == null)
            playerCamera = Camera.main;

        playerInput = inputManager.Input;
        EnableInput();
    }

    public void EnterDrone()
    {
        inputManager.SetActionMap(inputManager.Input.Drone);
        playerCamera.enabled = false;
        droneCamera.enabled = true;
    }

    public void ExitDrone()
    {
        inputManager.SetActionMap(inputManager.Input.Player);
        droneCamera.enabled = false;
        playerCamera.enabled = true;
    }

    private void OnExitInput(InputAction.CallbackContext _context)
    {
        if (_context.performed) ExitDrone();
    }

    private void OnCyclic(InputAction.CallbackContext _context)
    {
        cyclic = _context.ReadValue<Vector2>();
    }

    private void OnPedal(InputAction.CallbackContext _context)
    {
        pedal = _context.ReadValue<Vector2>();
    }

    private void OnThrottle(InputAction.CallbackContext _context)
    {
        throttle = _context.ReadValue<float>();
    }

    private void EnableInput()
    {
        exitInput = playerInput.Drone.Exit;
        exitInput.Enable();
        exitInput.performed += OnExitInput;

        cyclicInput = playerInput.Drone.Cyclic;
        cyclicInput.Enable();
        cyclicInput.performed += OnCyclic;

        pedalInput = playerInput.Drone.Cyclic;
        pedalInput.Enable();
        pedalInput.performed += OnPedal;

        throttleInput = playerInput.Drone.Cyclic;
        throttleInput.Enable();
        throttleInput.performed += OnThrottle;
    }

    private void OnDisable()
    {
        cyclicInput.Disable();
        pedalInput.Disable();
        throttleInput.Disable();
    }
}
