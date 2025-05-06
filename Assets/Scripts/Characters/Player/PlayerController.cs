using Interfaces;
using Inventory.Model;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(InputsEvents))]
    public class PlayerController : MonoBehaviour
    {
        [Header("PlayerMovement")] [SerializeField]
        private float JumpHeight = 1.2f;

        [SerializeField] private float Gravity = -9.8f;
        [SerializeField] private float FallTimeout = 0.15f;
        [SerializeField] private float SpeedChangeRate = 10.0f;
        [SerializeField] private float GroundedOffset = -0.41f;
        [SerializeField] private LayerMask GroundLayers;

        [HideInInspector] public bool Grounded = true;

        [Header("Cinemachine")] [SerializeField]
        private GameObject CinemachineCameraTarget;

        [SerializeField] private float TopClamp = 89.9f;
        [SerializeField] private float BottomClamp = -89.9f;
        [SerializeField] private float CameraAngleOverride = 0.0f;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _fallTimeoutDelta;

        private CharacterController _characterController;
        private InputsEvents _inputEvents;
        private PlayerView _playerView;

        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private LayerMask _raycastLayers;

        private const float _threshold = 0.01f;

        private Character _character;

        public static PlayerController Instance;
        
        public FiniteInventory inventory;

        private void Awake()
        {
            Instance = this;
            _character = GetComponent<Character>();
            _characterController = GetComponent<CharacterController>();
            _inputEvents = GetComponent<InputsEvents>();
            _playerView = GetComponent<PlayerView>();
            inventory = GetComponent<FiniteInventory>();
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _fallTimeoutDelta = FallTimeout;
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
            bool wasGrounded = Grounded;

            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, _characterController.radius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (!wasGrounded && Grounded)
            {
                _playerView.Landed();
            }

            _playerView.SetGrounded(Grounded);
        }

        private void CeilingCheck()
        {
            if (_verticalVelocity <= 0f) return;

            Vector3 spherePosition =
                new Vector3(transform.position.x, transform.position.y + _characterController.height, transform.position.z);

            if (Physics.CheckSphere(spherePosition, _characterController.radius, GroundLayers, QueryTriggerInteraction.Ignore))
            {
                _verticalVelocity = 0f;
            }
        }

        private void CameraRotation()
        {
            if (_inputEvents.GetLook.sqrMagnitude >= _threshold)
            {
                float deltaTimeMultiplier = 1.0f; //arreglar para mando

                _cinemachineTargetYaw += _inputEvents.GetLook.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _inputEvents.GetLook.y * deltaTimeMultiplier;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);

            transform.rotation = Quaternion.Euler(0.0f, _cinemachineTargetYaw, 0.0f);
        }

        private void HorizontalMovement()
        {
            float speed = _inputEvents.IsSprinting ? 2 * _character.speed : _character.speed;

            if (_inputEvents.GetMovement == Vector2.zero) speed = 0.0f;

            float currentHorizontalSpeed =
                new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;

            if (currentHorizontalSpeed < speed - speedOffset || currentHorizontalSpeed > speed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, speed,
                    Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = speed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, speed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 cameraForward = _mainCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = _mainCamera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            Vector3 inputDirection = cameraForward * _inputEvents.GetMovement.y + cameraRight * _inputEvents.GetMovement.x;

            if (inputDirection.sqrMagnitude > 1f)
            {
                inputDirection.Normalize();
            }

            // Movement
            _characterController.Move(inputDirection * (_speed * Time.deltaTime) +
                                      new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            _playerView.SetSpeed(Vector3.Dot(inputDirection, cameraForward) * _speed,
                Vector3.Dot(inputDirection, cameraRight) * _speed);
        }


        private void VerticalMovement()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;
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

            _playerView.SetVerticalSpeed(Grounded ? 0 : _verticalVelocity);

            //Gravity
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        public void Jump()
        {
            if (!Grounded) return;
            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
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
            Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out RaycastHit hit, distance,
                _raycastLayers);
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