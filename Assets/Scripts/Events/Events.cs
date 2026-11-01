using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Action OnEvent;
    
    private void Awake()
    {
        OnEvent += Function1;
        OnEvent += Function2;
    }

    private void Function1()
    {
        Debug.Log("Event1");
    }

    private void Function2()
    {
        Debug.Log("Event2");
    }

    private void OnDestroy()
    {
        OnEvent -= Function1;
        OnEvent -= Function2;
    }
}
