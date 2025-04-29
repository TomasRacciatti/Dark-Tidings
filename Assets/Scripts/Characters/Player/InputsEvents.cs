using UnityEngine;
using UnityEngine.InputSystem;
using Inventory.Controller;

namespace Characters.Player
{
    public class InputsEvents : MonoBehaviour
    {
        private InputActions inputActions;
        private PlayerController _playerController;

        [Header("Character Input Values")] public Vector2 movement;
        public Vector2 look;
        public bool sprint;
        public bool use;
        public bool inventoryOpened;
        public bool toggleBackpack;

        [Header("Movement Settings")] public bool analogMovement;

        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private void Awake()
        {
            inputActions = new InputActions();
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            inputActions.Player.Movement.performed += Movement;
            inputActions.Player.Movement.canceled += Movement;
            inputActions.Player.Look.performed += Look;
            inputActions.Player.Look.canceled += Look;
            inputActions.Player.Jump.performed += Jump;
            inputActions.Player.Jump.canceled += Jump;
            inputActions.Player.Sprint.performed += Sprint;
            inputActions.Player.Sprint.canceled += Sprint;
            inputActions.Player.Use.performed += StartUse;
            inputActions.Player.Use.canceled += StopUse;
            inputActions.Player.Interact.performed += Interact;
            inputActions.Player.ToggleInventory.performed += ToggleInventory;
            inputActions.Player.ToggleBackpack.performed += ToggleBackpack;
            inputActions.Player.Toolbar1.performed += SelectToolbar1;
            inputActions.Player.Toolbar2.performed += SelectToolbar2;
            inputActions.Player.Toolbar3.performed += SelectToolbar3;
            inputActions.Player.Toolbar4.performed += SelectToolbar4;
            inputActions.Enable();
        }

        private void OnDisable() //fijarme al final si esto esta igual q arriba con -
        {
            inputActions.Disable();
            inputActions.Player.Movement.performed -= Movement;
            inputActions.Player.Movement.canceled -= Movement;
            inputActions.Player.Look.performed -= Look;
            inputActions.Player.Look.canceled -= Look;
            inputActions.Player.Jump.performed -= Jump;
            inputActions.Player.Jump.canceled -= Jump;
            inputActions.Player.Sprint.performed -= Sprint;
            inputActions.Player.Sprint.canceled -= Sprint;
            inputActions.Player.Use.performed -= StartUse;
            inputActions.Player.Use.canceled -= StopUse;
            inputActions.Player.Interact.performed -= Interact;
            inputActions.Player.ToggleInventory.performed -= ToggleInventory;
            inputActions.Player.ToggleBackpack.performed -= ToggleBackpack;
            inputActions.Player.Toolbar1.performed -= SelectToolbar1;
            inputActions.Player.Toolbar2.performed -= SelectToolbar2;
            inputActions.Player.Toolbar3.performed -= SelectToolbar3;
            inputActions.Player.Toolbar4.performed -= SelectToolbar4;
        }

        public void Movement(InputAction.CallbackContext context)
        {
            movement = context.ReadValue<Vector2>();
        }

        public void Look(InputAction.CallbackContext context)
        {
            look = context.ReadValue<Vector2>();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                _playerController.Jump();
            }
        }

        public void Sprint(InputAction.CallbackContext context)
        {
            sprint = context.ReadValueAsButton();
        }

        public void StartUse(InputAction.CallbackContext context)
        {
            use = context.ReadValueAsButton();
            ItemsInHand.Instance.Use();
        }

        public void Interact(InputAction.CallbackContext context)
        {
            _playerController.Interact();
        }

        public void StopUse(InputAction.CallbackContext context)
        {
            use = context.ReadValueAsButton();
        }

        public void ToggleInventory(InputAction.CallbackContext context)
        {
            inventoryOpened = !inventoryOpened;
            CanvasGameManager.Instance.inventoryManager.ToggleInventory(inventoryOpened);
        }

        public void ToggleBackpack(InputAction.CallbackContext context)
        {
            toggleBackpack = !toggleBackpack;
            CanvasGameManager.Instance.inventoryManager.ToggleBackpack(toggleBackpack);
        }

        public void SelectToolbar1(InputAction.CallbackContext context) => SelectToolbar(0);
        public void SelectToolbar2(InputAction.CallbackContext context) => SelectToolbar(1);
        public void SelectToolbar3(InputAction.CallbackContext context) => SelectToolbar(2);
        public void SelectToolbar4(InputAction.CallbackContext context) => SelectToolbar(3);

        private void SelectToolbar(int index)
        {
            CanvasGameManager.Instance.inventoryManager.toolbar.ChangeSelectedSlot(index);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}