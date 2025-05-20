using System.Collections.Generic;
using Items.Base;
using Patterns.ObjectPool;
using TMPro;
using UnityEngine;

namespace Characters
{
    public class TargetDummy : Character
    {
        [SerializeField] private GameObject _indicator;
    
        public override void TakeDamage(int damage, List<SO_Item> modifiers = null)
        {
            string modifierNames = "";

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    modifierNames += $"{modifier.name} ,";
                }
            }
            else
            {
                modifierNames = "None";
            }
            

            // Mostrar indicador
            GameObject obj = Instantiate(_indicator, transform.position + Vector3.up * 1, Quaternion.identity);
            var text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"Damage: {damage}\nModifiers: {modifierNames}";
            Destroy(obj,5);
        }
    
        protected override void Death()
        {
            //cannot death
        }
    }
}
