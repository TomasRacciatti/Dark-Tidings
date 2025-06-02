using System;
using Characters.Player;
using Interfaces;
using Items.Base;
using UnityEngine;

namespace Inventory.Model
{
    public class BoltInventory : FiniteInventory
    {
        [SerializeField] private SO_Item SO_bullet;
        
        
        protected override void NotifyItemChanged(int index)
        {
            base.NotifyItemChanged(index);
            if(index != 3) UpdateCrafting();
        }
        
        private void UpdateCrafting()
        {
            ItemAmount item0 = Items[0];
            ItemAmount item1 = Items[1];
            ItemAmount item2 = Items[2];

            // Condición base: ambos ítems existen y son iguales
            bool baseValid = !item0.IsEmpty && !item1.IsEmpty && item0.SoItem == item1.SoItem;

            // Condición modificador válido: item2 es null o distinto del base
            bool modifierValid = item2.IsEmpty || item2.SoItem != item0.SoItem;

            if (baseValid && modifierValid)
            {
                ItemAmount bullet = new ItemAmount(SO_bullet, 5);
                bullet.AddModifier(new ItemAmount(!item2.IsEmpty ? item2.SoItem : item0.SoItem, 1));
                SetItemByIndex(3, bullet);
            }
            else
            {
                SetItemByIndex(3,new ItemAmount(null, 0));
            }
        }

        private void OnDisable()
        {
            
        }
    }
}