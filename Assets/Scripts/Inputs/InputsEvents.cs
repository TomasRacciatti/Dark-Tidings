using Players;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputsEvents : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputActions inputActions;
        private Controller _controller;
    
        [Header("Character Input Values")]
        public Vector2 movement;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool shoot;
        public bool toggleBackpack;
        
        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;
    
        private void Awake()
        {
            inputActions = new InputActions();
            _controller = GetComponent<Controller>();
            _playerInput = GetComponent<PlayerInput>();
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
            inputActions.Player.Toolbar1.performed += SelectToolbar1;
            inputActions.Player.Toolbar2.performed += SelectToolbar2;
            inputActions.Player.Toolbar3.performed += SelectToolbar3;
            inputActions.Player.Toolbar4.performed += SelectToolbar4;
            inputActions.Enable();
        }
        
        private void OnDisable()
        {
            inputActions.Disable();
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
            jump = context.ReadValueAsButton();
        }
    
        public void Sprint(InputAction.CallbackContext context)
        {
            sprint = context.ReadValueAsButton();
        }
    
        public void StartUse(InputAction.CallbackContext context)
        {
            shoot = context.ReadValueAsButton();
        }
        
        public void StopUse(InputAction.CallbackContext context)
        {
            shoot = context.ReadValueAsButton();
        }

        public void SelectToolbar1(InputAction.CallbackContext context) => SelectToolbar(0);
        public void SelectToolbar2(InputAction.CallbackContext context) => SelectToolbar(1);
        public void SelectToolbar3(InputAction.CallbackContext context) => SelectToolbar(2);
        public void SelectToolbar4(InputAction.CallbackContext context) => SelectToolbar(3);
        
        private void SelectToolbar(int index)
        {
            _controller.inventory.ChangeSelectedSlot(index);
        }
        
        public void ToggleBackpack(InputAction.CallbackContext context)
        {
            toggleBackpack = context.ReadValueAsButton();
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
