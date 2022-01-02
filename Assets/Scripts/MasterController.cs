using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
using PlayerController;

[CreateAssetMenu(fileName = "New Controller Manager", menuName = "Manager/Controller")]
public class MasterController : ScriptableObject
{
    public IController primaryController { get; private set; }
    public IController secondaryController { get; private set; }

    public void TryEnterPrimary(IController _controller)
    {
        if (_controller == primaryController)
            return;

        if (_controller.CanEnter())
        {
            if (primaryController != null)
            {
                if (primaryController.CanExit())
                    primaryController.OnExit();
                else return;
            }
                
            primaryController = _controller;
            _controller.OnEnter();
        }
    }

    public void TryEnterSecondary(IController _controller)
    {
        if (_controller == secondaryController)
            return;

        if (_controller.CanEnter())
        {
            if (secondaryController != null)
            {
                if (secondaryController.CanExit())
                    secondaryController.OnExit();
                else return;
            }

            secondaryController = _controller;
            _controller.OnEnter();
        }
    }
}
