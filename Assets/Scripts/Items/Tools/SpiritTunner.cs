using System;
using Items.Base;
using Objects.Clues;
using UnityEngine;

namespace Items.Tools
{
    public class SpiritTunner : Tool
    {
        public LayerMask clueLayerMask;
    
    
        [SerializeField] private MeshRenderer screen;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material detectionMaterial;
        [SerializeField] private Animator animator;

        public override void Use(UseType useType)
        {
        
        }

        private void OnEnable()
        {
            animator.SetTrigger("Spirit");
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
}
