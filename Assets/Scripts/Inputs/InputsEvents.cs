using Players;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputsEvents : MonoBehaviour
    {
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
            _controller = GetComponent<Controller>();
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
    
        public void Shoot(InputAction.CallbackContext context)
        {
            shoot = context.ReadValueAsButton();
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
