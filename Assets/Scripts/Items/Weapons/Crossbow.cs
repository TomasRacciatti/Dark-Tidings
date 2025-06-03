using System.Collections.Generic;
using Inventory.Model;
using Items.Base;
using Managers;
using Patterns;
using Patterns.ObjectPool;
using UnityEngine;

namespace Items.Weapons
{
    public class Crossbow : ItemEquippable
    {
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private SO_Item _bolt;

        private ItemAmount _boltType = new();
        private ItemAmount _boltRequired1 = new();
        private ItemAmount _boltRequired2 = new();
        private Cooldown _cooldown = new();

        public override void Use()
        {
            Fire();
        }

        private void Load(ItemAmount boltType)
        {
            if (boltType.SoItem == null || boltType.Modifiers == null)
            {
                Debug.LogWarning("No bolt loaded!");
                return;
            }
            _boltType = boltType;
            Debug.Log("Loaded: " + _boltType.ItemName);
        }
        
        private void Fire()
        {/*
            if (_boltType.SoItem == null || _boltType.Modifiers == null)
            {
                Debug.LogWarning("No bolt loaded!");
                return;
            }*/
            if (!_cooldown.IsReady) return;

            InventorySystem inventorySystem = GameManager.Player.inventory;
            if (inventorySystem == null) return;

            ItemAmount bolt = inventorySystem.GetFirstSoItem(_bolt);
            if (bolt.IsEmpty) return;

            // Creamos una copia con cantidad 1 para removerla
            ItemAmount boltToRemove = new ItemAmount(bolt.SoItem, 1, bolt.Modifiers);
            inventorySystem.RemoveItem(ref boltToRemove);

            // Disparo
            GameObject boltInstance = ObjectPoolManager.Instance.SpawnObject(_boltPrefab, _firePoint.position, _firePoint.rotation, 10f);
            boltInstance.GetComponent<Bolt>().SetModifiers(bolt.Modifiers);

            Debug.Log("Fired: " + _boltType.ItemName);
            _cooldown.StartCooldown(1);
        }
    }
}