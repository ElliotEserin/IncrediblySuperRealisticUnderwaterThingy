using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    int numberOfPickups = 0;
    int maxPickups;
    public Text pickupText;
    public string prefix = "Pickups collected: ";

    static Manager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        maxPickups = FindObjectsOfType<Pickup>().Length;
        pickupText.text = prefix + numberOfPickups + "/" + maxPickups;
       
    }

    public static void AddPickup()
    {
        instance.numberOfPickups++;
        instance.pickupText.text = instance.prefix + instance.numberOfPickups + "/" + instance.maxPickups;
        FindObjectOfType<PlayerMovement>().speed += 0.5f;
    }
    
}
