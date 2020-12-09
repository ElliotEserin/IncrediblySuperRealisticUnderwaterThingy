using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Light pointLight;
    public float intensityStrength;
    public float scaleStrength;
    public float speed;

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<Manager>().eMessage.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<Manager>().eMessage.SetActive(false);
    }

    private void OnDestroy()
    {
        FindObjectOfType<Manager>().eMessage.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.E))
            {
                Manager.AddPickup();
                Destroy(gameObject);
                Debug.Log("Picked up collecatble");
            }
        }
    }

    private void Update()
    {
        var value = Mathf.Sin(Time.time * speed);
        pointLight.intensity = value * intensityStrength + intensityStrength;
        //transform.localScale = Vector3.one * (value * scaleStrength + (scaleStrength*2));
    }
}
