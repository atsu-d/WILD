using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    bool CanEnter();
    bool CanExit();
    void OnEnter();
    void OnExit();
}