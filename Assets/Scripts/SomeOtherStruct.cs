using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct SomeOtherStruct
    {
        [SerializeField] private float _float;
        [SerializeField] private string _string;

        public override string ToString() => $"_float: {_float}, _string: {_string}";
    }
}