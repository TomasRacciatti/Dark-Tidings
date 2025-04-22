using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private Vector3 _movement;

    private void Update()
    {
        transform.position += _movement * Time.deltaTime;
    }
}
