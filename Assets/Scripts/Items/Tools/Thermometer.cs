using Objects.Clues;
using UnityEngine;
using Interfaces;

namespace Items.Tools
{
    public class Thermometer : Tool
    {
        private float _temperature = 15;
        public LayerMask clueLayerMask;

        [SerializeField] private MeshRenderer screen;

        [SerializeField] private Material highMaterial;
        [SerializeField] private Material lowMaterial;
        [SerializeField] private Material negativeMaterial;
    
        public override void Use()
        {
        
        }

        private void Update()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1, clueLayerMask);
            foreach (var hit in hits)
            {
                Clue clue = hit.GetComponent<Clue>();
                if (clue)
                {
                    SO_Clue type = clue.GetClueProvided;
                    if (type is SO_ThermClue tempClue)
                    {
                        var (minTemp, maxTemp) = tempClue.GetValue;
                        float targetTemp = Random.Range(minTemp, maxTemp);
                        _temperature = Mathf.Lerp(_temperature, targetTemp, Time.deltaTime);
                        SetMaterial();
                        return;
                    }
                }
            }

            _temperature = Mathf.Lerp(_temperature, 15f, Time.deltaTime);
            SetMaterial();
        }

        private void SetMaterial()
        {
            screen.material = _temperature switch
            {
                >= 10 => highMaterial,
                >= 0 => lowMaterial,
                _ => negativeMaterial
            };
        }
    }
}
