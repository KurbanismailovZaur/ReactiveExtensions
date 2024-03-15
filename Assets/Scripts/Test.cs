using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Codomaster.ReactiveExtensions;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Test : MonoBehaviour
{
    [SerializeField] private ReactiveProperty<float> _floatProperty;

    [SerializeField] private ReactiveProperty<SomeStruct> _someStructProperty;
    
    [SerializeField] private ReactiveProperty<UnityEngine.Object> _someObjectProperty;
    
    [SerializeField] private ReactiveProperty<SomeScriptableObject> _someScriptableObjectProperty;

    public void OnFloatPropertyChanged(float value) => print(value);

    public void OnSomeStructPropertyChanged(SomeStruct someStruct) => print(someStruct);

    public void OnSomeObjectPropertyChanged(UnityEngine.Object obj) => print(obj);

    public void OnSomeScriptableObjectPropertyChanged(SomeScriptableObject someScriptableObject) => print(someScriptableObject);
}
