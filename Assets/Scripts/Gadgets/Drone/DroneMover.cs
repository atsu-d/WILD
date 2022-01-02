using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DroneController))]
public class DroneMover : MonoBehaviour
{
    private DroneController input;
    private Rigidbody rb;
    [SerializeField] private float mass = 1f;
    protected float initialDrag, initialAngularDrag;

    [Header("Control Properties")]
    private float minMaxPitch = 30f, minMaxRoll = 30f;
    private float pitch, roll, yaw;
    [SerializeField] private float lerpSpeed = 1.2f;

    [Header("Engine Properties")]
    private List<IMotor> motors = new List<IMotor>();

    [Header("Drone Camera Properties")]
    private Transform droneCam;
    private float lookVertical, lastVertical, lastYaw;
    private float cameraSpeed = 115f;
    [SerializeField] private float lookThreshold = 1.1f;
    [SerializeField] private float inputMultiplier = 0.0033f;
    [Range(0f, 90f)]
    public float upperVerticalLimit = 60f;
    [Range(0f, 90f)]
    public float lowerVerticalLimit = 60f;
    [Range(1f, 25f)]
    public float verticalSmooth = 5f;
    [Range(1f, 25f)]
    public float horizontalSmooth = 2.5f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb)
        {
            rb.mass = mass;
            initialDrag = rb.drag;
            initialAngularDrag = rb.angularDrag;
        }
    }

    private void Start()
    {
        input = GetComponent<DroneController>();
        droneCam = GetComponentInChildren<Camera>().transform;
        motors = GetComponentsInChildren<IMotor>().ToList();

        foreach (IMotor _motor in motors)
        {
            _motor.OnInitializeMotor();
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) 
            return;

        HandlePhysics();
    }

    protected virtual void HandlePhysics()
    {
        HandleEngine();
        HandleControls();
    }

    protected virtual void HandleEngine()
    {
        foreach(IMotor _motor in motors)
        {
            _motor.OnUpdateMotor(rb, input);
        }
    }

    protected virtual void HandleControls()
    {
        float _forward = input.Cyclic.y;
        float _pitch = _forward * minMaxPitch;

        float _roll = -input.Cyclic.x * minMaxRoll;

        float _newHorizontalAngle = GetLookAxis(input.Pedal.x); ;
        lastYaw = Mathf.Lerp(lastYaw, _newHorizontalAngle, Time.deltaTime * horizontalSmooth);
        yaw += lastYaw * cameraSpeed * Time.deltaTime;

        pitch = Mathf.Lerp(pitch, _pitch, Time.deltaTime * lerpSpeed);
        roll = Mathf.Lerp(roll, _roll, Time.deltaTime * lerpSpeed);
        Quaternion _rotation = Quaternion.Euler(pitch, yaw, roll);
        rb.MoveRotation(_rotation);

        HandleCamRotation(_forward != 0);
    }

    protected virtual void HandleCamRotation(bool _isPitching)
    {
        if (_isPitching)
        {
            float _level = lookVertical;
            _level = Mathf.Lerp(_level, 0, Time.deltaTime * lerpSpeed);
            lookVertical = _level;
        }
        else
        {
            float _newVerticalInput = GetLookAxis(-input.Pedal.y);

            lastVertical = Mathf.Lerp(lastVertical, _newVerticalInput, Time.deltaTime * verticalSmooth);
            lookVertical += lastVertical * cameraSpeed * Time.deltaTime;
        }

        lookVertical = Mathf.Clamp(lookVertical, -upperVerticalLimit, lowerVerticalLimit);

        #region Update Rotation
        droneCam.localRotation = Quaternion.Euler(new Vector3(lookVertical, 0, 0));
        #endregion
    }

    private float GetLookAxis(float _axis)
    {
        //Get raw mouse input;
        float _input = _axis;

        if (_input > -lookThreshold && _input < lookThreshold) return 0;
        //Since raw mouse input is already time-based, we need to correct for this before passing the input to the camera controller;
        if (Time.timeScale > 0f && Time.deltaTime > 0f)
        {
            _input /= Time.deltaTime;
            _input *= Time.timeScale;
        }
        else
            _input = 0f;

        //Apply mouse sensitivity;
        _input *= inputMultiplier;

        return _input;
    }
}
