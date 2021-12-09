using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int", menuName = "Variable/Int")]

public class StaticInt : ScriptableObject
{
    [SerializeField] private int value;
    public int Value => value;

    public void Select(int _selection)
    {
        value = _selection;
    }
}
