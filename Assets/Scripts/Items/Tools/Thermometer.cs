using System;
using System.Collections;
using System.Collections.Generic;
using Objects.Clues;
using UnityEngine;

public class Thermometer : Tool
{
    private float temperature = 25;
    public LayerMask clueLayerMask;

    [SerializeField] private MeshRenderer screen;
    
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highMaterial;
    [SerializeField] private Material lowMaterial;
    [SerializeField] private Material negativeMaterial;
    
    public override void Use()
    {
        
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position,1, clueLayerMask);
        foreach (var hit in hits)
        {
            Clue clue = hit.GetComponent<Clue>();
            if (clue != null)
            {
                SO_Clue type = clue.GetClueProvided;
                if (type is SO_ThermClue tempClue)
                {
                    temperature = Mathf.Lerp(temperature, tempClue.GetTemperature(), Time.deltaTime);
                    SetMaterial();
                    return;
                }
            }
        }
        temperature = Mathf.Lerp(temperature, 25, Time.deltaTime);
        SetMaterial();
    }

    private void SetMaterial()
    {
        if (temperature >= 20)
        {
            screen.material = defaultMaterial;
        }
        else if (temperature >= 10)
        {
            screen.material = highMaterial;
        }
        else if (temperature >= 0)
        {
            screen.material = lowMaterial;
        }
        else
        {
            screen.material = negativeMaterial;
        }
    }
}
