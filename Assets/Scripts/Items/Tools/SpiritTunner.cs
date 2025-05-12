using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Clues;

public class SpiritTunner : Tool
{
    public LayerMask clueLayerMask;
    
    
    [SerializeField] private MeshRenderer screen;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material detectionMaterial;
    
    public override void Use()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetClue();
    }

    private void SetClue()
    {
        int cantClues = 0;
        
        Collider[] hits = Physics.OverlapSphere(transform.position,1, clueLayerMask);
        foreach (var hit in hits)
        {
            Clue clue = hit.GetComponent<Clue>();
            if (clue != null)
            {
                cantClues++;
            }
        }

        if (cantClues > 0)
        {
            screen.material = detectionMaterial;
        }
        else
        {
            screen.material = defaultMaterial;
        }
    }
}
