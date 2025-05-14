using System;
using System.Collections;
using UnityEngine;
using Interfaces;
using Inventory.Controller;
using Inventory.Model;
using Items;

namespace Objects
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private float openedAngle = -120f;

        [SerializeField] private float closedAngle = 0f;

        //[SerializeField] private float timeAnim = 2f;
        [SerializeField] private AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private bool isOpen = false;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private SO_Item keyItem;

        private Quaternion closedRotation;
        private Quaternion openRotation;

        //private Coroutine currentCoroutine;
        private HingeJoint hinge;

        [SerializeField] private Transform
            interactionPoint; // Me permite settear el lugar donde quiero que esten los pop ups. Si no modifico esto me va a agarrar el transform default

        public Transform InteractionPoint => interactionPoint != null ? interactionPoint : transform;

        private void Awake()
        {
            hinge = GetComponent<HingeJoint>();
        }

        void Start()
        {
            closedRotation = transform.rotation;
            openRotation = transform.rotation * Quaternion.Euler(0f, openedAngle, 0f);

            Setup();
        }

        private void Setup()
        {
            transform.rotation = isOpen ? openRotation : closedRotation;
            
            JointLimits limits = hinge.limits;
            limits.min = !isLocked ? openedAngle : closedAngle;
            limits.max = closedAngle;
            hinge.limits = limits;

            JointSpring spring = hinge.spring;
            spring.targetPosition = !isLocked ? isOpen ? openedAngle : closedAngle : closedAngle;
            hinge.spring = spring;
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
                LockDoor(!isLocked);
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
            JointSpring spring = hinge.spring;
            spring.targetPosition = isOpen ? openedAngle : closedAngle;
            hinge.spring = spring;
        }

        private void LockDoor(bool locked)
        {
            if (isOpen) return;
            isLocked = locked;
        }

        private IEnumerator ForceDoor()
        {
            float duration = 0.5f;
            float magnitude = 2f;

            Quaternion originalRotation = transform.rotation;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float shake = Mathf.Sin(elapsed * 40f) * magnitude;
                transform.localRotation = originalRotation * Quaternion.Euler(0f, shake, 0f);
                yield return null;
            }

            transform.localRotation = originalRotation;
        }
    }
}