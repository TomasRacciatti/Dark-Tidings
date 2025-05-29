using System.Linq;

namespace Items.Base
{
    public static class ItemsUtility
    {
        public static bool AreStackable(ItemAmount a, ItemAmount b)
        {
            if (a.IsEmpty || b.IsEmpty) return false;

            //if (a.IsFull || b.IsFull) return false; // sacar esta si se quiere hacer que el stack se mantenga en destino
            if (a.SoItem != b.SoItem) return false;

            if (a.Modifiers.Count != b.Modifiers.Count) return false;

            for (int i = 0; i < a.Modifiers.Count; i++)
            {
                if (a.Modifiers[i].SoItem != b.Modifiers[i].SoItem)
                    return false;
            }

            return true;
        }
    }
}