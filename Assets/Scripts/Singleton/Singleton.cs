using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> : MonoBehaviour where T : Component 
{
    public static T instance;

    public bool HasInstance => instance != null;    

    public T Instance 
    { 
        get
        {
            if (HasInstance)
                return Instance;
            else
            {

                T instance = FindAnyObjectByType<T>();
                if (instance != null)
                    return instance;
                else
                {
                    GameObject instanceObject = new GameObject(nameof(T));
                    T createdInstance = instanceObject.AddComponent<T>();
                    return createdInstance;
                }
            }

        } 
    }
    private void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(gameObject);
    }

}
