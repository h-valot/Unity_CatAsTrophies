using System;
using UnityEngine;

namespace Misc
{
    public class WrapperVariable<T> : ScriptableObject
    {

        private T _value;
        public T value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke();
            }
        }
    
        public event Action OnChanged;
    }
}
