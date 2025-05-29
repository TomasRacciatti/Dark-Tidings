using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Inventory.Controller;
using Items.Base;
using Managers;

namespace Characters.Player
{
    public class InputsEvents : MonoBehaviour
    {
        private InputActions _inputActions;
        private PlayerController _playerController;

        private Vector2 _movement;
        private Vector2 _look;
        private bool _sprint;
        private bool _use;
        private bool _inventoryOpened;
        private bool _toggleBackpack;
        
        public Vector2 GetMovement => !_inventoryOpened && !GameManager.Paused ? _movement: Vector2.zero;
        public Vector2 GetLook => !_inventoryOpened && !GameManager.Paused ? _look : Vector2.zero;
        public bool IsSprinting => _sprint;
        public bool IsUsing => _use;
        public bool InventoryOpened => _inventoryOpened;

        private Toolbar _toolbar;

        private void Awake()
        {
            _inputActions = new InputActions();
            _playerController = GetComponent<PlayerController>();
            _toolbar = GetComponent<Toolbar>();
        }

        private void OnEnable()
        {
            _inputActions.Player.Movement.performed += Movement;
            _inputActions.Player.Movement.canceled += Movement;
            _inputActions.Player.Look.performed += Look;
            _inputActions.Player.Look.canceled += Look;
            _inputActions.Player.Jump.performed += Jump;
            _inputActions.Player.Jump.canceled += Jump;
            _inputActions.Player.Sprint.performed += Sprint;
            _inputActions.Player.Sprint.canceled += Sprint;
            _inputActions.Player.Use.performed += StartUse;
            _inputActions.Player.Use.canceled += StopUse;
            _inputActions.Player.Interact.performed += Interact;
            _inputActions.Player.Inventory.performed += Inventory;
            _inputActions.Player.Journal.performed += Journal;
            _inputActions.Player.Toolbar1.performed += SelectToolbar1;
            _inputActions.Player.Toolbar2.performed += SelectToolbar2;
            _inputActions.Player.Toolbar3.performed += SelectToolbar3;
            _inputActions.Player.Toolbar4.performed += SelectToolbar4;
            _inputActions.Player.Pause.performed += Pause;
            _inputActions.Enable();
        }

        private void OnDisable() //fijarme al final si esto esta igual q arriba con -
        {
            _inputActions.Disable();
            _inputActions.Player.Movement.performed -= Movement;
            _inputActions.Player.Movement.canceled -= Movement;
            _inputActions.Player.Look.performed -= Look;
            _inputActions.Player.Look.canceled -= Look;
            _inputActions.Player.Jump.performed -= Jump;
            _inputActions.Player.Jump.canceled -= Jump;
            _inputActions.Player.Sprint.performed -= Sprint;
            _inputActions.Player.Sprint.canceled -= Sprint;
            _inputActions.Player.Use.performed -= StartUse;
            _inputActions.Player.Use.canceled -= StopUse;
            _inputActions.Player.Interact.performed -= Interact;
            _inputActions.Player.Inventory.performed -= Inventory;
            _inputActions.Player.Journal.performed -= Journal;
            _inputActions.Player.Toolbar1.performed -= SelectToolbar1;
            _inputActions.Player.Toolbar2.performed -= SelectToolbar2;
            _inputActions.Player.Toolbar3.performed -= SelectToolbar3;
            _inputActions.Player.Toolbar4.performed -= SelectToolbar4;
            _inputActions.Player.Pause.performed -= Pause;
        }

        private void Movement(InputAction.CallbackContext context)
        {
            _movement = context.ReadValue<Vector2>();
        }

        private void Look(InputAction.CallbackContext context)
        {
            _look = context.ReadValue<Vector2>();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && context.ReadValueAsButton())
            {
                _playerController.Jump();
            }
        }

        private void Sprint(InputAction.CallbackContext context)
        {
            _sprint = context.ReadValueAsButton();
        }

        private void StartUse(InputAction.CallbackContext context)
        {
            _use = context.ReadValueAsButton();
            if (!_inventoryOpened && !GameManager.Paused)
            {
                ItemsInHand.Instance.Use();
            }
        }
        
        private void StopUse(InputAction.CallbackContext context)
        {
            _use = context.ReadValueAsButton();
        }

        private void Interact(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused)
            {
                _playerController.Interact();
            }
        }

        private void Inventory(InputAction.CallbackContext context)
        {
            InventoryUI(0);
        }

        private void Journal(InputAction.CallbackContext context)
        {
            InventoryUI(1);
        }
        
        private void InventoryUI(int targetIndex)
        {
            if (GameManager.Paused) return;

            var currentIndex = GameManager.Canvas.inventoryManager.GetIndexInventory();
            
            ItemDescription.Hide(); //esto modificarlo despues
            
            if (currentIndex == targetIndex || targetIndex == -1)
            {
                GameManager.Canvas.inventoryManager.InventorySwitcherUI.gameObject.SetActive(false);
                GameManager.SetCursorVisibility(false);
                _inventoryOpened = false;
                return;
            }
            
            GameManager.Canvas.inventoryManager.InventorySwitcherUI.SwitchTo(targetIndex);
            GameManager.Canvas.inventoryManager.InventorySwitcherUI.gameObject.SetActive(true);
            GameManager.SetCursorVisibility(true);
            _inventoryOpened = true;
        }
        
        private void Pause(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && _inventoryOpened)
            {
                InventoryUI(-1);
                return;
            }
            GameManager.TogglePause();
        }

        private void SelectToolbar1(InputAction.CallbackContext context) => SelectToolbar(0);
        private void SelectToolbar2(InputAction.CallbackContext context) => SelectToolbar(1);
        private void SelectToolbar3(InputAction.CallbackContext context) => SelectToolbar(2);
        private void SelectToolbar4(InputAction.CallbackContext context) => SelectToolbar(3);

        private void SelectToolbar(int index)
        {
            _toolbar.SetSelectedSlot(index);
        }
    }
}