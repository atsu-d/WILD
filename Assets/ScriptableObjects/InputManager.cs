using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Input Manager", menuName = "Manager/Input Manager")]
public class InputManager : ScriptableObject
{
    public PlayerInput Input { get; private set; }

    public event Action<InputActionMap> OnInputChange;
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

    public void SetPlayerInput(PlayerInput _input)
    {
        Input = _input;
    }
}
