using System;
using System.Collections.Generic;
using Inventory.Model;
using Items.Base;
using Managers;
using Managers.ObjectPool;
using UnityEngine;

namespace Items.Weapons
{
    public class Crossbow : ItemEquippable
    {
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private SO_Item _bolt;

        private ItemAmount _boltType = new();
        private InventorySystem _boltInventorySystem;

        private void Start()
        {
            _boltInventorySystem = GameManager.Canvas.inventoryManager.boltsInventorySystem;
        }

        public override void Use(UseType useType)
        {
            switch (useType)
            {
                case UseType.Default:
                    Fire();
                    break;
                case UseType.Aim:
                    break;
                case UseType.Reload1:
                    Reload1();
                    break;
                case UseType.Reload2:
                    Reload2();
                    break;
                case UseType.Reload3:
                    _boltInventorySystem.TransferIndexToIndex(_boltInventorySystem, 0, 1);
                    Reload2();
                    break;
            }
        }
        
        private void Reload1()
        {
            Debug.Log("Reload animation started: pulling string back.");
            // Aquí pondrías tu animación, por ejemplo:
            // animator.SetTrigger("PullString");
        }
        
        private void Reload2()
        {
            ItemAmount bolt = _boltInventorySystem.GetFirstSoItem(_bolt);
            if (bolt.IsEmpty) return;

            // Creamos una copia con cantidad 1 para removerla
            _boltType = new ItemAmount(bolt);
            _boltType.SetAmount(1);
            
            ItemAmount boltToRemove = new ItemAmount(bolt.SoItem, 1, bolt.Modifiers);
            _boltInventorySystem.RemoveItem(ref boltToRemove);
        }
        
        private void Fire()
        {
            if (_boltType.IsEmpty)
            {
                return;
            }
            
            GameObject boltInstance = ObjectPoolManager.Instance.SpawnObject(_boltPrefab, _firePoint.position, _firePoint.rotation, 10f);
            boltInstance.GetComponent<Bolt>().SetModifiers(_boltType.Modifiers);

            _boltType = new ItemAmount();
        }
    }
}