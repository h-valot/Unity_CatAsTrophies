using System;
using UnityEngine;

namespace Misc
{
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

    public class WrapperAction<T1, T2> : ScriptableObject
    {
        public event Action<T1, T2> action;

        public void Call(T1 t1, T2 t2) => action?.Invoke(t1, t2);
    }
}