using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCam.transform.forward);
    }
}
