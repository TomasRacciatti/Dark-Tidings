using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Backpack : MonoBehaviour, IInventory
{
    [SerializeField] private BackpackStats _backpackStats;
    
    private List<object> _items = new List<object>();
    private Transform _backpackTransform;
    private bool _isEquipped = true;
    private Transform _playerTransform;
    private GameObject _backpackInstance;
    
    [Header("Backpack Placement Settings")]
    [SerializeField] private Vector3 _equippedPosition = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 _equippedRotation = new Vector3(0, 0, 0);
    [SerializeField] private Transform _backpackAttachPoint;
    
    private void Awake()
    {
        if (_backpackStats == null)
        {
            Debug.LogError("BackpackStats not assigned to Backpack component!");
            return;
        }
        
        // Create backpack instance
        CreateBackpackInstance();
    }
    
    private void CreateBackpackInstance()
    {
        if (_backpackStats.BackpackPrefab == null) return;
        
        _backpackInstance = Instantiate(_backpackStats.BackpackPrefab, transform);
        _backpackTransform = _backpackInstance.transform;
        
        // By default, the backpack is equipped
        EquipBackpack();
    }
    
    public void EquipBackpack()
    {
        if (!_isEquipped && _backpackInstance != null)
        {
            _backpackInstance.transform.SetParent(_backpackAttachPoint != null ? _backpackAttachPoint : transform);
            _backpackInstance.transform.localPosition = _equippedPosition;
            _backpackInstance.transform.localRotation = Quaternion.Euler(_equippedRotation);
            _isEquipped = true;
            
            // Notify player that backpack is equipped (apply debuffs)
            SendMessageUpwards("OnBackpackEquipped", _backpackStats, SendMessageOptions.DontRequireReceiver);
        }
    }
    
    public void UnequipBackpack()
    {
        if (_isEquipped && _backpackInstance != null)
        {
            // Place the backpack on the ground, slightly in front of the player
            Vector3 groundPosition = transform.position + transform.forward * 1f;
            groundPosition.y = 0; // Assuming y=0 is the ground
            
            _backpackInstance.transform.SetParent(null); // Detach from player
            _backpackInstance.transform.position = groundPosition;
            _isEquipped = false;
            
            // Notify player that backpack is unequipped (remove debuffs)
            SendMessageUpwards("OnBackpackUnequipped", SendMessageOptions.DontRequireReceiver);
        }
    }
    
    public bool IsEquipped()
    {
        return _isEquipped;
    }
    
    public BackpackStats GetBackpackStats()
    {
        return _backpackStats;
    }
    
    // IInventory Implementation
    public void AddItem(object item)
    {
        _items.Add(item);
    }

    public bool RemoveItem(object item)
    {
        return _items.Remove(item);
    }

    public List<object> GetItems()
    {
        return _items;
    }

    public bool HasItem(object item)
    {
        return _items.Contains(item);
    }

    public void Clear()
    {
        _items.Clear();
    }
}