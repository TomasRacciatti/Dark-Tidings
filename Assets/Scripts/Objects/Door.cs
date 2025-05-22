using System;
using System.Collections;
using UnityEngine;
using Interfaces;
using Inventory.Controller;
using Items.Base;

namespace Objects
{
    public class Door : MonoBehaviour, IInteractable, IPushable
    {
        [Header("Options")] [SerializeField] private float openedAngle = 120f;
        [SerializeField] private float closedAngle = 0f;
        [SerializeField] private float lockedAngle = 3f;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private SO_Item keyItem;

        [SerializeField] private Transform interactionPoint;
        [SerializeField] private GameObject noExploit;
        
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closeSound;
        [SerializeField] private AudioClip forcedSound;
        [SerializeField] private AudioClip lockedSound;
        
        private HingeJoint _hinge;
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        private float _hingeForce;
        private float _lastOpenedAngle;

        public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;

        private void Awake()
        {
            _hinge = GetComponent<HingeJoint>();
            _audioSource = GetComponent<AudioSource>();
            _rigidbody = GetComponent<Rigidbody>();
            _hingeForce = _hinge.spring.spring;
            _lastOpenedAngle = -openedAngle;
        }

        void Start()
        {
            transform.rotation = isOpen ? transform.rotation * Quaternion.Euler(0f, _lastOpenedAngle, 0f) : transform.rotation;
            noExploit.SetActive(isLocked);
            Setup();
        }

        private void Setup()
        {
            JointLimits limits = _hinge.limits;
            limits.min = !isLocked ? -openedAngle : -lockedAngle;
            limits.max = !isLocked ? openedAngle : lockedAngle;
            _hinge.limits = limits;

            JointSpring spring = _hinge.spring;
            spring.targetPosition = isLocked || !isOpen ? closedAngle : _lastOpenedAngle;
            _hinge.spring = spring;
        }

        public void Interact(GameObject interactableObject)
        {
            if (isOpen)
            {
                ToggleDoor();
                return;
            }

            Toolbar toolbar = interactableObject.GetComponent<Toolbar>();
            if (toolbar.GetSlotItem().SoItem == keyItem)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.y, closedAngle)) <= 5f)
                {
                    LockDoor(!isLocked);
                }
                return;
            }

            if (isLocked)
            {
                StartCoroutine(ForceDoor());
                return;
            }
            
            Vector3 directionToPlayer = (interactableObject.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, directionToPlayer);
            _lastOpenedAngle = dot >= 0 ? openedAngle : -openedAngle;

            ToggleDoor();
        }

        private void ToggleDoor()
        {
            isOpen = !isOpen;
            _audioSource.PlayOneShot(isOpen ? openSound : closeSound);
            Setup();
        }

        private void LockDoor(bool locked)
        {
            if (isOpen) return;
            isLocked = locked;
            noExploit.SetActive(isLocked);
            _audioSource.PlayOneShot(lockedSound);
            Setup();
        }

        private IEnumerator ForceDoor()
        {
            _audioSource.PlayOneShot(forcedSound);

            JointLimits limits = _hinge.limits;
            limits.min = -lockedAngle;
            limits.max = lockedAngle;
            _hinge.limits = limits;

            JointSpring spring = _hinge.spring;
            var force = spring.spring;
            spring.spring = 5000;

            float elapsed = 0f;
            float duration = 1.2f;

            while (elapsed < duration && !isOpen)
            {
                elapsed += Time.deltaTime;

                float shake = Mathf.Sin(elapsed * 25) * lockedAngle;
                float target = Mathf.Clamp(closedAngle + shake, -lockedAngle, lockedAngle);

                spring.targetPosition = target;
                _hinge.spring = spring;

                yield return null;
            }
            
            spring.targetPosition = closedAngle;
            spring.spring = _hingeForce;
            _hinge.spring = spring;
            Setup();
        }
        
        public void OnPushed(Vector3 pushDirection, float strength)
        {
            if (_rigidbody == null) return;
            if (isLocked) return;

            StartCoroutine(PlayIfMoved());
        }
        
        private IEnumerator PlayIfMoved()
        {
            yield return new WaitForFixedUpdate();

            if (_rigidbody.angularVelocity.magnitude > 0.1f && !_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(openSound);
            }
        }
    }
}