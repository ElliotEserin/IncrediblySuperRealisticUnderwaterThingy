using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Rise");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + Vector3.up * y + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
