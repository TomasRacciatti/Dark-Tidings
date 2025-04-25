using UnityEngine;

namespace Items
{
    public class MaterialDecorator : ItemDecorator
    {
        [SerializeField] private ItemObject materialObject;
        public ItemObject GetMaterialObject() => materialObject;

        public override string GetItemName()
        {
            return $"{materialObject.GetItemName()} {base.GetItemName()}";
        }

        public override string GetFullDescription()
        {
            return $"{base.GetFullDescription()}\nMade of: {materialObject.GetItemName()}";
        }
    }
}