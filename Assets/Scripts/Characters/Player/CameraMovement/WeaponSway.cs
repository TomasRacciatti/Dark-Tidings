using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")] 
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;
    [SerializeField] private float _rollMultiplier;
    
    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * _swayMultiplier;
        
        var rotation = CalculateRotation(mouseDelta);
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, _smooth * Time.deltaTime);
    }


    private Quaternion CalculateRotation(Vector2 mouseDelta)
    {
        // Input del mouse
        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;
        
        // Calculamos rotacion
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        
        // Roll
        float rollAmount = -mouseX * _rollMultiplier;
        Quaternion rotationZ = Quaternion.AngleAxis(rollAmount, Vector3.forward);
        
        // Rotacion final
        Quaternion targetRotation = rotationX * rotationY * rotationZ;
        
        return targetRotation;
    }
}
