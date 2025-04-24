using System;
using System.Collections;
using UnityEngine;
using Interfaces;
using Inventory.Model;
using Items;

namespace Objects
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private float openedAngle = -120f;
        [SerializeField] private float closedAngle = 0f;
        [SerializeField] private float timeAnim = 2f;
        [SerializeField] private AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private bool isOpen = false;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private ItemObject keyItem;

        private Quaternion closedRotation;
        private Quaternion openRotation;
        private Coroutine currentCoroutine;
        private HingeJoint hingeJoint;

        private void Awake()
        {
            hingeJoint = GetComponent<HingeJoint>();
        }
        
        void Start()
        {
            closedRotation = transform.rotation;
            openRotation = transform.rotation * Quaternion.Euler(0f, openedAngle, 0f);
            
            transform.rotation = isOpen ? openRotation : closedRotation;
            
            JointLimits limits = hingeJoint.limits;
            limits.min = openedAngle;
            limits.max = closedAngle;
            hingeJoint.limits = limits;

            JointSpring spring = hingeJoint.spring;
            spring.targetPosition = isOpen ? openedAngle : closedAngle;
            hingeJoint.spring = spring;
        }

        public void Interact(GameObject interactableObject)
        {
            /*if (isLocked)
            {
                InventorySystem inventory = interactableObject.GetComponent<InventorySystem>();
                if (inventory.HasItem(keyItem))
                {
                    LockDoor(false);
                }
                else
                {
                    if (currentCoroutine != null)
                        StopCoroutine(currentCoroutine);

                    currentCoroutine = StartCoroutine(ForceDoor());
                    return;
                }
            }*/
        
            isOpen = !isOpen;
            JointSpring spring = hingeJoint.spring;
            spring.targetPosition = isOpen ? openedAngle : closedAngle;
            hingeJoint.spring = spring;

            /*
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(RotateDoor());*/
        }
    
        public void LockDoor(bool locked)
        {
            if (isOpen) return;
            isLocked = locked;
        }

        private IEnumerator RotateDoor()
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;

            float time = 0f;
            while (time < timeAnim)
            {
                time += Time.deltaTime;
                float curveT = openCurve.Evaluate(time / timeAnim);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, curveT);
                yield return null;
            }

            transform.rotation = targetRotation;
            currentCoroutine = null;
        }
    
        private IEnumerator ForceDoor()
        {
            float duration = 0.3f;
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