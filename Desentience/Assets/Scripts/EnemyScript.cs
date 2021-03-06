﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;
    public Transform robotModel;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(h, 0, v) * speed;

        robotModel.rotation = Quaternion.Euler(v * 10.0f, 0f, -h * 10.0f);
    }
}
