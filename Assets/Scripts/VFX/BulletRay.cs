using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRay : MonoBehaviour
{
    public float speed = 100;
    TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        trail.Clear();
    }

    private void OnDisable()
    {
        trail.Clear();
    }

    void Update()
    {
        transform.position += transform.forward * (Time.deltaTime * speed);
    }
}