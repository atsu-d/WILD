using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input Manager", menuName = "Manager/Input Manager")]
public class InputManager : ScriptableObject
{
    public PlayerInput Input { get; private set; }
    private InputActionMap playerMap;
    private InputActionMap droneMap;

    public event Action<InputActionMap> OnInputChange;

    public bool playerInputEnabled => Input.Player.enabled;
    public bool droneInputEnabled => Input.Drone.enabled;
    
    public void Initialize(PlayerInput _input)
    {
        Input = _input;
        playerMap = Input.Player;
        droneMap = Input.Drone;
        ActivatePlayerControls();
    }

    public void ActivatePlayerControls()
    {
        Input.Drone.Disable();
        Input.Player.Enable();

        Debug.Log("Player: " + Input.Player.enabled + " / Drone: " + Input.Drone.enabled);
    }

    public void ActivateDroneControls()
    {
        Input.Player.Disable();
        Input.Drone.Enable();
        Debug.Log("Player: " + Input.Player.enabled + " / Drone: " + Input.Drone.enabled);
    }


    public void SetActionMap(InputActionMap _actionMap)
    {
        if (_actionMap.enabled)
            return;

        Input.Disable();
        if (OnInputChange != null) 
            OnInputChange(_actionMap);
        _actionMap.Enable();

        Debug.Log("Enabled");
    }
}
