using UnityEngine;

namespace Patterns
{
    public class Switcher : MonoBehaviour
    {
        [SerializeField] private int activeIndex = 0;

        void Start()
        {
            SwitchTo(activeIndex);
        }

        public void SwitchTo(int index)
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(i == index);
            }

            activeIndex = index;
        }

        public void Next()
        {
            int childCount = transform.childCount;
            int nextIndex = (activeIndex + 1) % childCount;
            SwitchTo(nextIndex);
        }

        public void Previous()
        {
            int childCount = transform.childCount;
            int prevIndex = (activeIndex - 1 + childCount) % childCount;
            SwitchTo(prevIndex);
        }

        public int GetCurrentIndex()
        {
            return activeIndex;
        }
    }
}
