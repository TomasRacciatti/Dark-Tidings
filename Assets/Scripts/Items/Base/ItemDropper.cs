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
        
        public static bool IsActive => _instance.gameObject.activeSelf;

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

            Drop(fromItemUI.itemAmount);
            
            fromSlotUI.InventoryUI.InventorySystem.SetItemByIndex(fromSlotUI.SlotIndex, new ItemAmount());
            Hide();
        }

        public static void Drop(ItemAmount itemAmount)
        {
            GameObject itemObject = Instantiate(_instance.itemPrefab, GameManager.Player.transform.position + 1.2f * Vector3.up,
                Quaternion.identity);
            itemObject.GetComponent<ItemPrefab>().SetItemAmount(new ItemAmount(itemAmount));
            
            Rigidbody rb = itemObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce((GameManager.Player.transform.forward + 0.8f * Vector3.up).normalized * 3.5f, ForceMode.Impulse);
            }
        }
    }
}