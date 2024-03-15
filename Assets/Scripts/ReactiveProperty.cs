using System;
using UnityEngine;
using UnityEngine.Events;

namespace Codomaster.ReactiveExtensions
{
    [Serializable]
    public struct ReactiveProperty<T>
    {
        [Serializable] public class ChangedEvent : UnityEvent<T> { }
        
        [SerializeField] private T _value;
        
        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                    return;

                _value = value;
                Changed?.Invoke(_value);
            }
        }

        public ChangedEvent Changed;

        public ReactiveProperty(T value = default)
        {
            _value = default;
            Changed = new ChangedEvent();
        }

        private void InvokeChangedEvent() => Changed?.Invoke(_value);
    }
}