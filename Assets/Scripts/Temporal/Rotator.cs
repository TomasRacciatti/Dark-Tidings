using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float rotationSpeed = 90f;

    // Eje normal al eje vertical (Y)
    public Vector3 rotationAxis = Vector3.forward; // Puedes usar Vector3.right también

    void Update()
    {
        // Rotar constantemente alrededor del eje definido
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
