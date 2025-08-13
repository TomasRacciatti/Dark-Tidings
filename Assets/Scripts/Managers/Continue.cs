using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class Continue : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(GameManager.Canvas.FinishGame());
    }
}
