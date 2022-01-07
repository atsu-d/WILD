using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ItemSystem;

public class DroneController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    public DroneUseBehavior droneObject;
    public Camera droneCamera;
    public Camera playerCamera;

    private PlayerInput playerInput;
    private InputAction cyclicInput, pedalInput, throttleInput, exitInput;

    private Vector2 cyclic, pedal;
    private float throttle;

    public Vector2 Cyclic { get => cyclic; }
    public Vector2 Pedal { get => pedal; }
    public float Throttle { get => throttle; }

    public Rigidbody rb { get; private set; }

    private void Awake()
    {
        if (droneCamera == null)
            droneCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
            playerCamera = Camera.main;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerInput = inputManager.Input;
    }

    private void Update()
    {
        if (!inputManager.droneInputEnabled)
            return;

        cyclic = cyclicInput.ReadValue<Vector2>();
        pedal = pedalInput.ReadValue<Vector2>();
        throttle = throttleInput.ReadValue<float>();
    }

    public void EnterDrone()
    {
        playerCamera.enabled = false;
        droneCamera.enabled = true;

        inputManager.ActivateDroneControls();
        EnableInput();
    }

    public void ExitDrone()
    {
        if (droneCamera == null)
            droneCamera = GetComponentInChildren<Camera>();
        
        droneCamera.enabled = false;
        playerCamera.enabled = true;

        inputManager.ActivatePlayerControls();
        DisableInput();
    }

    public bool DroneControlsEnabled()
    {
        return inputManager.droneInputEnabled;
    }

    private void OnExitInput(InputAction.CallbackContext _context)
    {
        if (_context.performed) ExitDrone();
    }

    private void EnableInput()
    {
        if (!inputManager.Input.Drone.enabled)
            return;
        
        if (playerInput == null)
            playerInput = inputManager.Input;

        exitInput = playerInput.Drone.Exit;
        exitInput.Enable();
        exitInput.performed += OnExitInput;

        cyclicInput = playerInput.Drone.Cyclic;
        cyclicInput.Enable();

        pedalInput = playerInput.Drone.Pedal;
        pedalInput.Enable();

        throttleInput = playerInput.Drone.Throttle;
        throttleInput.Enable();
    }

    private void OnEnable()
    {
        droneObject.drone = this;

    }
    private void OnDisable()
    {
        if (!inputManager.Input.Drone.enabled)
            return;

        DisableInput();
    }

    private void DisableInput()
    {
        if (playerInput == null)
            playerInput = inputManager.Input;

        pedalInput.Disable();
        cyclicInput.Disable();
        throttleInput.Disable();
    }
}
