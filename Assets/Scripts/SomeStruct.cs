using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public struct SomeStruct
    {
        [SerializeField] private int _integer;
        [SerializeField] private bool _boolean;
        [SerializeField] private SomeOtherStruct _someOtherStruct;

        public override string ToString() => $"_integer: {_integer}, _boolean: {_boolean}, _someOtherStruct: {_someOtherStruct}";
    }
}