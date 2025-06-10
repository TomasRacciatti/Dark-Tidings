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
        
        public Vector2 GetMovement => !InventoryOpened && !GameManager.Paused ? _movement: Vector2.zero;
        public Vector2 GetLook => !InventoryOpened && !GameManager.Paused ? _look : Vector2.zero;
        public bool IsSprinting => _sprint;
        public bool InventoryOpened => Switcher.GetIndexSwitcher(GameManager.Canvas.inventoryManager.InventorySwitcherUI) != -1;

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
            _inputActions.Player.Use.performed += Use;
            _inputActions.Player.Interact.performed += Interact;
            _inputActions.Player.Inventory.performed += Inventory;
            _inputActions.Player.Journal.performed += Journal;
            _inputActions.Player.Toolbar1.performed += SelectToolbar1;
            _inputActions.Player.Toolbar2.performed += SelectToolbar2;
            _inputActions.Player.Toolbar3.performed += SelectToolbar3;
            _inputActions.Player.Toolbar4.performed += SelectToolbar4;
            _inputActions.Player.Pause.performed += Pause;
            _inputActions.Player.Reload.started += ReloadStarted;
            _inputActions.Player.Reload.canceled += ReloadCanceled;
            _inputActions.Player.Reload.performed += ReloadPerformed;
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
            _inputActions.Player.Use.performed -= Use;
            _inputActions.Player.Interact.performed -= Interact;
            _inputActions.Player.Inventory.performed -= Inventory;
            _inputActions.Player.Journal.performed -= Journal;
            _inputActions.Player.Toolbar1.performed -= SelectToolbar1;
            _inputActions.Player.Toolbar2.performed -= SelectToolbar2;
            _inputActions.Player.Toolbar3.performed -= SelectToolbar3;
            _inputActions.Player.Toolbar4.performed -= SelectToolbar4;
            _inputActions.Player.Pause.performed -= Pause;
            _inputActions.Player.Reload.started -= ReloadStarted;
            _inputActions.Player.Reload.canceled -= ReloadCanceled;
            _inputActions.Player.Reload.performed -= ReloadPerformed;
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

        private void Use(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && !InventoryOpened)
            {
                ItemsInHand.Use();
            }
        }
        
        private void Aim(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && !InventoryOpened)
            {
                ItemsInHand.Use(UseType.Aim);
            }
        }

        private void ReloadStarted(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && !InventoryOpened)
            {
                ItemsInHand.Use(UseType.Reload1); // inicia la animación del hilo
            }
        }

        private void ReloadCanceled(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && !InventoryOpened)
            {
                ItemsInHand.Use(UseType.Reload2); // se usa la misma flecha
            }
        }

        private void ReloadPerformed(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && !InventoryOpened)
            {
                ItemsInHand.Use(UseType.Reload3); // se cambia a la otra flecha
            }
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
            GameManager.Canvas.InventoryUI(0);
        }

        private void Journal(InputAction.CallbackContext context)
        {
            GameManager.Canvas.InventoryUI(1);
        }
        
        private void Pause(InputAction.CallbackContext context)
        {
            if (!GameManager.Paused && InventoryOpened)
            {
                GameManager.Canvas.InventoryUI(-1);
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
        
        private void Reload0(InputAction.CallbackContext context) => Reload(0);
        private void Reload1(InputAction.CallbackContext context) => Reload(1);
        private void Reload2(InputAction.CallbackContext context) => Reload(2);
        
        private void Reload(int index)
        {
            print(index);
        }
    }
}