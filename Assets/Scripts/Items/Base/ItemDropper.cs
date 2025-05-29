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

            GameObject itemObject = Instantiate(itemPrefab, GameManager.Player.transform.position, Quaternion.identity);
            itemObject.GetComponent<ItemPrefab>().SetItemAmount(fromItemUI.itemAmount);
            fromSlotUI.InventoryUI.Inventory.SetItemByIndex(fromSlotUI.SlotIndex, new ItemAmount());
            Hide();
        }
    }
}
