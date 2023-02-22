using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>
public class PersistanSingleton<T> : MonoBehaviour where T : PersistanSingleton<T>
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this as T;
        }
        DontDestroyOnLoad(this);
    }
}
