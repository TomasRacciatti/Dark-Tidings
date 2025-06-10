using Items.Base;

namespace Items
{
    public class KeyItem : ItemEquippable
    {
        public override void Use(UseType useType)
        {
            print("key");
        }
    }
}