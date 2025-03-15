using System;
using System.Reflection;
using UnityEngine;

public class DebugMonoBehaviour : MonoBehaviour
{
    void Start()
    {
        Type monoType = typeof(MonoBehaviour);
        MethodInfo[] methods = monoType.GetMethods();

        foreach (MethodInfo method in methods)
        {
            Debug.Log("Method: " + method.Name);
        }
    }
}
