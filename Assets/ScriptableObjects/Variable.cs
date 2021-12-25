using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable<T> : ScriptableObject
{
    public T value { get; private set; }

    public void Set(T _value)
    {
        value = _value;
    }
}
