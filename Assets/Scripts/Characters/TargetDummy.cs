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
    
        public override void TakeDamage(int damage, LinkedList<ItemAmount> modifiers = null)
        {
            float finalDamage = damage;

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    if (Contains(weaknesses,modifier))
                    {
                        finalDamage *= 2.5f;
                    }
                    else if (Contains(strengths, modifier))
                    {
                        finalDamage *= 0.2f;
                    }
                }
            }
            Debug.Log($"{gameObject.name} recibió {finalDamage} de daño");
            
            string modifierNames = "";

            if (modifiers != null && modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    modifierNames += $"{modifier.SoItem.name} ,";
                }
            }
            else
            {
                modifierNames = "None";
            }
            

            // Mostrar indicador
            GameObject obj = ObjectPoolManager.Instance.SpawnObject(_indicator, transform.position + Vector3.up * 1, Quaternion.identity, 5f);
            var text = obj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = $"Damage: {finalDamage}\nModifiers: {modifierNames}";
            //Destroy(obj,5);
        }
    
        protected override void Death()
        {
            //cannot death
        }
    }
}
