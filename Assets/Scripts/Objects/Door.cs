using System;
using System.Collections;
using UnityEngine;
using Interfaces;
using Inventory.Controller;
using Inventory.Model;
using Items;
using Items.Base;

namespace Objects
{
    public class Door : MonoBehaviour, IInteractable, IPushable
    {
        [Header("Options")] [SerializeField] private float openedAngle = -120f;
        [SerializeField] private float closedAngle = 0f;
        [SerializeField] private float lockedAngle = -5f;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private SO_Item keyItem;

        [SerializeField] private Transform interactionPoint;
        
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closeSound;
        [SerializeField] private AudioClip forcedSound;
        [SerializeField] private AudioClip lockedSound;
        
        private Quaternion _closedRotation;
        private Quaternion _openRotation;
        private HingeJoint _hinge;
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;

        public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;

        private void Awake()
        {
            _hinge = GetComponent<HingeJoint>();
            _audioSource = GetComponent<AudioSource>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Start()
        {
            _closedRotation = transform.rotation;
            _openRotation = transform.rotation * Quaternion.Euler(0f, openedAngle, 0f);
            transform.rotation = isOpen ? _openRotation : _closedRotation;
            Setup();
        }

        private void Setup()
        {
            JointLimits limits = _hinge.limits;
            limits.min = !isLocked ? openedAngle : lockedAngle;
            limits.max = closedAngle;
            _hinge.limits = limits;

            JointSpring spring = _hinge.spring;
            spring.targetPosition = !isLocked ? isOpen ? openedAngle : closedAngle : closedAngle;
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
            if (toolbar.GetSlotItem().Item == keyItem)
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

            ToggleDoor();
        }

        private void ToggleDoor()
        {
            isOpen = !isOpen;
            _audioSource.PlayOneShot(isOpen ? openSound : closeSound);
            JointSpring spring = _hinge.spring;
            spring.targetPosition = isOpen ? openedAngle : closedAngle;
            _hinge.spring = spring;
        }

        private void LockDoor(bool locked)
        {
            if (isOpen) return;
            isLocked = locked;
            _audioSource.PlayOneShot(lockedSound);
            Setup();
        }

        private IEnumerator ForceDoor()
        {
            _audioSource.PlayOneShot(forcedSound);

            JointLimits limits = _hinge.limits;
            limits.min = lockedAngle;
            limits.max = closedAngle;
            _hinge.limits = limits;

            JointSpring spring = _hinge.spring;
            var force = spring.spring;
            spring.spring = 500;

            float elapsed = 0f;
            float duration = 1.2f;

            while (elapsed < duration && !isOpen)
            {
                elapsed += Time.deltaTime;

                float shake = Mathf.Sin(elapsed * 5000f) * lockedAngle;
                float target = Mathf.Clamp(closedAngle + shake, closedAngle, lockedAngle);

                spring.targetPosition = target;
                _hinge.spring = spring;

                yield return null;
            }
            
            spring.targetPosition = closedAngle;
            spring.spring = force;
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