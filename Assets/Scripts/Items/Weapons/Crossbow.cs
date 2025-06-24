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
        [SerializeField] private float recoilStrength = 0.2f;

        private ItemAmount _boltType = new();
        private InventorySystem _inventorySystem;
        private InventorySystem _boltInventorySystem;

        private void Start()
        {
            _inventorySystem = GameManager.Player.inventory;
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
                    if (!_boltType.IsEmpty) return;
                    Reload2();
                    break;
                case UseType.Reload3:
                    if (!_boltType.IsEmpty) _boltInventorySystem.AddItem(ref _boltType);
                    if (!_boltType.IsEmpty) GameManager.Player.AddItem(ref _boltType);
                    _boltType = new ItemAmount();
                    _boltInventorySystem.TransferIndexToIndex(_boltInventorySystem, 0, 1);
                    Reload2();
                    break;
            }
        }
        
        private void Reload1()
        {
            if (!_boltType.IsEmpty) return;
            Debug.Log("Reload animation started:");
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
            
            CameraShake1.Instance.Shake(transform.right, recoilStrength);
            GameObject boltInstance = ObjectPoolManager.Instance.SpawnObject(_boltPrefab, _firePoint.position, _firePoint.rotation, 10f);
            boltInstance.GetComponent<Bolt>().SetModifiers(_boltType.Modifiers);

            _boltType = new ItemAmount();
        }
    }
}