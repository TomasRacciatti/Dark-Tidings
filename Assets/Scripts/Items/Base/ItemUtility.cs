namespace Items.Base
{
    public static class ItemUtility
    {
        public static bool AreStackable(ItemAmount a, ItemAmount b)
        {
            if (a == null || b == null) return false;
            if (a.IsEmpty || b.IsEmpty) return false;

            if (a.IsFull || b.IsFull) return false;
            if (a.SoItem != b.SoItem) return false;

            if (a.Modifiers.Count != b.Modifiers.Count) return false;

            var nodeA = a.Modifiers.First;
            var nodeB = b.Modifiers.First;

            while (nodeA != null && nodeB != null)
            {
                if (nodeA.Value.SoItem != nodeB.Value.SoItem)
                    return false;

                nodeA = nodeA.Next;
                nodeB = nodeB.Next;
            }

            return true;
        }
    }
}