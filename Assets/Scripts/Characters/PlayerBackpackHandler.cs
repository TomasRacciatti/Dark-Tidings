using System;
using Inputs;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerCharacter))]
public class PlayerBackpackHandler : MonoBehaviour
{
    /*
    private Controller _controller;
    private Player _player;
    private Backpack _backpack;
    
    private float _originalMoveSpeed;
    private float _originalSprintSpeed;
    private bool _isBackpackEquipped = true;
    private bool _wasTogglePressed = false;
    
    [Header("Input")]
    private InputsEvents _inputEvents;
    private void Awake()
    {
        _controller = GetComponent<Controller>();
        _player = GetComponent<Player>();
        _backpack = GetComponentInChildren<Backpack>();
        _inputEvents = GetComponent<InputsEvents>();
        
        if (_backpack == null)
        {
            Debug.LogWarning("No Backpack component found on player or children!");
        }
        
        // Store original values
        _originalMoveSpeed = _controller.MoveSpeed;
        _originalSprintSpeed = _controller.SprintSpeed;
    }
    
    private void Start()
    {
        // Apply backpack debuffs at start (since the backpack starts equipped)
        if (_backpack != null && _backpack.IsEquipped())
        {
            ApplyBackpackDebuffs(_backpack.GetBackpackStats());
        }
    }
    
    private void Update()
    {
        // Handle backpack toggle with the new input system
        if (_inputEvents != null && _inputEvents.toggleBackpack && !_wasTogglePressed)
        {
            ToggleBackpack();
            _wasTogglePressed = true;
        }
        else if (_inputEvents != null && !_inputEvents.toggleBackpack)
        {
            _wasTogglePressed = false;
        }
    }
    
    private void ToggleBackpack()
    {
        if (_backpack == null) return;
        
        if (_backpack.IsEquipped())
        {
            _backpack.UnequipBackpack();
        }
        else
        {
            _backpack.EquipBackpack();
        }
    }
    
    // Called by the Backpack component through SendMessage
    private void OnBackpackEquipped(BackpackStats backpackStats)
    {
        ApplyBackpackDebuffs(backpackStats);
    }
    
    // Called by the Backpack component through SendMessage
    private void OnBackpackUnequipped()
    {
        RemoveBackpackDebuffs();
    }
    
    private void ApplyBackpackDebuffs(BackpackStats backpackStats)
    {
        if (backpackStats == null) return;
        
        _isBackpackEquipped = true;
        
        // Apply speed debuff
        _controller.MoveSpeed = _originalMoveSpeed * backpackStats.SpeedMultiplier;
        _controller.SprintSpeed = _originalSprintSpeed * backpackStats.SpeedMultiplier;
        
        // We'll implement accuracy reduction later when shooting is implemented
        
        Debug.Log("Backpack debuffs applied - Speed reduced, damage taken increased");
    }
    
    private void RemoveBackpackDebuffs()
    {
        _isBackpackEquipped = false;
        
        // Restore original speed
        _controller.MoveSpeed = _originalMoveSpeed;
        _controller.SprintSpeed = _originalSprintSpeed;
        
        Debug.Log("Backpack debuffs removed - Speed and damage resistance restored");
    }
    
    // Override the TakeDamage method to apply the damage multiplier when the backpack is equipped
    public void ModifyIncomingDamage(ref int damage)
    {
        if (_isBackpackEquipped && _backpack != null)
        {
            BackpackStats backpackStats = _backpack.GetBackpackStats();
            if (backpackStats != null)
            {
                damage = Mathf.RoundToInt(damage * backpackStats.DamageTakenMultiplier);
            }
        }
    }*/
}