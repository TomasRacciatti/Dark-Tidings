using UnityEngine;

namespace Items
{
    public class ItemDecorator : ItemObject
    {
        [SerializeField] private ItemObject baseItem;

        public void SetBaseItem(ItemObject item)
        {
            baseItem = item;
        }

        public override string GetItemName()
        {
            return baseItem != null ? baseItem.GetItemName() : base.GetItemName();
        }

        public override string GetFullDescription()
        {
            return baseItem != null ? baseItem.GetFullDescription() : base.GetFullDescription();
        }
    }
}
