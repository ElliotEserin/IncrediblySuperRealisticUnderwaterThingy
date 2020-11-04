using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform mainCamera;
    public float speed;
    public float acceleration;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Rise");
        float z = Input.GetAxis("Vertical");
        //Vector3 move = transform.right * x + Vector3.up * y + transform.forward * z;
        Vector3 move = transform.right * x + Vector3.up * y + mainCamera.forward * z;
        Vector3 distance = Vector3.Lerp(controller.velocity, move * speed, acceleration * Time.deltaTime);
        controller.Move(distance * Time.deltaTime);

        //Debug
        if(Input.GetKeyDown(KeyCode.P)) Debug.Break();
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }
}
