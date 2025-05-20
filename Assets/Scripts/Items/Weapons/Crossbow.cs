using Items.Base;
using Patterns.ObjectPool;
using UnityEngine;

namespace Items.Weapons
{
    public class Crossbow : ItemEquippable
    {
        [SerializeField] private GameObject _boltPrefab;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private SO_Item _bolt;

        private ItemInstance _boltType = new();
        private ItemInstance _boltRequired1 = new();
        private ItemInstance _boltRequired2 = new();

        public override void Use()
        {
            Fire();
        }

        private void Load(ItemInstance boltType)
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
            
            GameObject boltInstance = ObjectPoolManager.Instance.SpawnObject(_boltPrefab, _firePoint.position, _firePoint.rotation, 10f);
            //boltInstance.GetComponent<Bolt>().SetModifiers(_boltType.Modifiers);
            Debug.Log("Fired: " + _boltType.ItemName);
        }
    }
}