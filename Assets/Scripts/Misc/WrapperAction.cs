using System;
using UnityEngine;

public class WrapperAction : ScriptableObject
{
    public event Action action;

    public void Call() => action?.Invoke();
}

public class WrapperAction<T> : ScriptableObject
{
    public event Action<T> action;

    public void Call(T t) => action?.Invoke(t);
}

public class WrapperAction<T, T2> : ScriptableObject
{
    public event Action<T, T2> action;

    public void Call(T t, T2 t2) => action?.Invoke(t, t2);
}
