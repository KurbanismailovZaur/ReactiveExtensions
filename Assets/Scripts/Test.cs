using System.Collections;
using System.Collections.Generic;
using Codomaster.ReactiveExtensions;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Test : MonoBehaviour
{
    [SerializeField] private ReactiveProperty<float> _floatProperty;

    [SerializeField] private ReactiveProperty<SomeStruct> _someStructProperty;
    
    public void OnFloatPropertyChanged(float value)
    {
        transform.position = new Vector3(0f, 0f, value);
    }

    public void OnSomeStructPropertyChanged(SomeStruct someStruct)
    {
        print(someStruct);
    }
}
