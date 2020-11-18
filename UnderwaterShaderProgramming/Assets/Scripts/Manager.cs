using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    int numberOfPickups = 0;

    static Manager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public static void AddPickup()
    {
        instance.numberOfPickups++;
    }
}
