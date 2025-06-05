using Interfaces;
using Inventory.Controller;
using Inventory.Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputsEvents))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerMovement")]
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float fallTimeout = 0.15f;
        [SerializeField] private float speedChangeRate = 10.0f;
        [SerializeField] private float groundedOffset = -0.21f;
        [SerializeField] private LayerMask groundLayers;

        private bool _grounded = true;
        public bool Grounded => _grounded;

        [Header("Cinemachine")] [SerializeField]
        private GameObject cinemachineCameraTarget;

        [SerializeField] private float topClamp = 89.9f;
        [SerializeField] private float bottomClamp = -89.9f;
        [SerializeField] private float cameraAngleOverride = 0.0f;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private const float TerminalVelocity = 53.0f;

        // timeout deltatime
        private float _fallTimeoutDelta;

        private CharacterController _characterController;
        private InputsEvents _inputEvents;
        private PlayerView _playerView;
        private Character _character;

        [SerializeField] private GameObject mainCamera;
        [SerializeField] private LayerMask raycastLayers;

        private const float Threshold = 0.01f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputEvents = GetComponent<InputsEvents>();
            _playerView = GetComponent<PlayerView>();
            _character = GetComponent<Character>();
        }

        private void Start()
        {
            _cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _fallTimeoutDelta = fallTimeout;
        }

        private void Update()
        {
            GroundedCheck();
            CeilingCheck();
            VerticalMovement();
            HorizontalMovement();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            bool wasGrounded = _grounded;

            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _characterController.radius, groundLayers,
                QueryTriggerInteraction.Ignore);

            if (!wasGrounded && _grounded)
            {
                _playerView.Landed();
            }

            _playerView.SetGrounded(_grounded);
        }

        private void CeilingCheck()
        {
            if (_verticalVelocity <= 0f) return;

            Vector3 spherePosition =
                new Vector3(transform.position.x, transform.position.y + _characterController.height,
                    transform.position.z);

            if (Physics.CheckSphere(spherePosition, _characterController.radius, groundLayers,
                    QueryTriggerInteraction.Ignore))
            {
                _verticalVelocity = 0f;
            }
        }

        private void CameraRotation()
        {
            if (_inputEvents.GetLook.sqrMagnitude >= Threshold)
            {
                float deltaTimeMultiplier = 1.0f; //arreglar para mando

                _cinemachineTargetYaw += _inputEvents.GetLook.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _inputEvents.GetLook.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + cameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);

            transform.rotation = Quaternion.Euler(0.0f, _cinemachineTargetYaw, 0.0f);
        }

        private void HorizontalMovement()
        {
            float tempSpeed = _inputEvents.IsSprinting ? _character.Stats.SprintMultiplier * _character.Stats.MovementSpeed : _character.Stats.MovementSpeed;

            if (_inputEvents.GetMovement == Vector2.zero) tempSpeed = 0.0f;

            float currentHorizontalSpeed =
                new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;

            if (currentHorizontalSpeed < tempSpeed - speedOffset || currentHorizontalSpeed > tempSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, tempSpeed,
                    Time.deltaTime * speedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = tempSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, tempSpeed, Time.deltaTime * speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 cameraForward = mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = mainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            Vector3 inputDirection =
                cameraForward * _inputEvents.GetMovement.y + cameraRight * _inputEvents.GetMovement.x;

            if (inputDirection.sqrMagnitude > 1f)
            {
                inputDirection.Normalize();
            }

            // Movement
            _characterController.Move(inputDirection * (_speed * Time.deltaTime) +
                                      new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            _playerView.SetSpeed(_speed,Vector3.Dot(inputDirection, cameraForward) * _speed,
                Vector3.Dot(inputDirection, cameraRight) * _speed);
        }


        private void VerticalMovement()
        {
            if (_grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = fallTimeout;
                _playerView.SetGrounded(true);
                _playerView.SetFalling(false);

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -3f;
                }
            }
            else
            {
                // fall timeout
                if (_fallTimeoutDelta >= 0.0f && _verticalVelocity < 0)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _playerView.SetFalling(true);
                }
            }

            _playerView.SetVerticalSpeed(_grounded ? 0 : _verticalVelocity);

            //Gravity
            if (_verticalVelocity < TerminalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
        }

        public void Jump()
        {
            if (!_grounded) return;
            _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _playerView.Jumped();
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public RaycastHit GetCameraRay(int distance = 1000)
        {
            Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hit, distance,
                raycastLayers);
            return hit;
        }

        public void Interact()
        {
            RaycastHit hit = GetCameraRay(3);
            if (hit.collider)
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}