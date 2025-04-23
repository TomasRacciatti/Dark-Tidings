using System.Collections;
using UnityEngine;
using Interfaces;
using Inventory.Model;
using Items;

namespace Objects
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform doorPivot;
        [SerializeField] private float openAngle = -90f;
        [SerializeField] private float timeAnim = 2f;
        [SerializeField] private AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private bool isOpen = false;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private ItemObject keyItem;

        private Quaternion closedRotation;
        private Quaternion openRotation;
        private Coroutine currentCoroutine;

        void Start()
        {
            closedRotation = doorPivot.rotation;
            openRotation = doorPivot.rotation * Quaternion.Euler(0f, openAngle, 0f);
            doorPivot.rotation = isOpen ? openRotation : closedRotation;
        }

        public void Interact(GameObject interactableObject)
        {
            if (isLocked)
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
            }
        
            isOpen = !isOpen;

            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

            currentCoroutine = StartCoroutine(RotateDoor());
        }
    
        public void LockDoor(bool locked)
        {
            if (isOpen) return;
            isLocked = locked;
        }

        private IEnumerator RotateDoor()
        {
            Quaternion startRotation = doorPivot.rotation;
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;

            float time = 0f;
            while (time < timeAnim)
            {
                time += Time.deltaTime;
                float curveT = openCurve.Evaluate(time / timeAnim);
                doorPivot.rotation = Quaternion.Lerp(startRotation, targetRotation, curveT);
                yield return null;
            }

            doorPivot.rotation = targetRotation;
            currentCoroutine = null;
        }
    
        private IEnumerator ForceDoor()
        {
            float duration = 0.3f;
            float magnitude = 2f;

            Quaternion originalRotation = doorPivot.rotation;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float shake = Mathf.Sin(elapsed * 40f) * magnitude;
                doorPivot.localRotation = originalRotation * Quaternion.Euler(0f, shake, 0f);
                yield return null;
            }

            doorPivot.localRotation = originalRotation;
        }
    }
}