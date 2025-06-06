using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float headbobAmount = 0.05f;
    [SerializeField] private float frequency = 10f;
    [SerializeField] private float smooth = 10f;

    [SerializeField] private InputActionReference moveAction;

    private Vector3 startPos;

    private void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }
    
    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.action.Disable();
    }

    private void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        CheckForHeadbobTrigger();
        StopHeadbob();
    }

    private void CheckForHeadbobTrigger()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        
        float inputMagnitude = moveInput.magnitude;

        if (inputMagnitude > 0)
            StartHeadbob();
    }

    private Vector3 StartHeadbob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * frequency) * headbobAmount * 1.4f, Time.deltaTime * smooth);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * frequency / 2) * headbobAmount * 1.6f, Time.deltaTime * smooth);
        transform.localPosition += pos;

        return pos;
    }

    private void StopHeadbob()
    {
        if (transform.localPosition == startPos)
            return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime);
    }
}
