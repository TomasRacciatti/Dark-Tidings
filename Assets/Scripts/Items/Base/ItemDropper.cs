using Inventory.View;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items.Base
{
    public class ItemDropper : MonoBehaviour, IDropHandler
    {
        private static ItemDropper _instance;
        [SerializeField] private GameObject itemPrefab;

        private void Awake()
        {
            _instance = this;
            Hide();
        }

        public static void Show()
        {
            _instance.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            _instance.gameObject.SetActive(false);
        }

        public void OnDrop(PointerEventData eventData)
        {
            ItemUI fromItemUI = eventData.pointerDrag.GetComponent<ItemUI>();
            SlotUI fromSlotUI = fromItemUI.GetComponentInParent<SlotUI>();
            if (fromSlotUI == null) return;

            GameObject itemObject = Instantiate(itemPrefab, GameManager.Player.transform.position + 1.2f * Vector3.up,
                Quaternion.identity);
            itemObject.GetComponent<ItemPrefab>().SetItemAmount(fromItemUI.itemAmount);
            
            Rigidbody rb = itemObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce((GameManager.Player.transform.forward + 0.8f * Vector3.up).normalized * 3.5f, ForceMode.Impulse);
            }
            
            fromSlotUI.InventoryUI.Inventory.SetItemByIndex(fromSlotUI.SlotIndex, new ItemAmount());
            Hide();
        }
    }
}