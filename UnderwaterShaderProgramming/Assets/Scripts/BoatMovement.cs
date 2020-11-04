using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public float xSpeed, ySpeed, zSpeed;
    public float xStrength, yStrength, zStrength;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Sin(xSpeed, xStrength), Sin(ySpeed, yStrength), Sin(zSpeed, zStrength));
    }

    float Sin(float speed, float strength)
    {
        return Mathf.Sin(Time.time * speed) * strength;
    }
}
