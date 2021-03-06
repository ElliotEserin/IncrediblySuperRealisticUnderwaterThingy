﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform mainCamera;
    public float speed;
    public float acceleration;

    public GameObject godRay;

    public PlayerAudio playerAudio;
    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (UIManager.paused)
            return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Rise");
        float z = Input.GetAxis("Vertical");
        anim.SetFloat("Vertical", z);

        if (z != 0)
            controller.radius = 0.85f;
        else
            controller.radius = 0.5f;

        //Vector3 move = transform.right * x + Vector3.up * y + transform.forward * z;
        Vector3 move = transform.right * x + Vector3.up * y + mainCamera.forward * z;
        Vector3 distance = Vector3.Lerp(controller.velocity, move * speed, acceleration * Time.deltaTime);
        controller.Move(distance * Time.deltaTime);

        //Audio
        if (move.magnitude > 0)
            playerAudio.PlaySwimAudio();

        //move particles
        var pos = transform.position; pos.y = godRay.transform.position.y;
        godRay.transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Escape)) FindObjectOfType<UIManager>().Pause();
    }
}
