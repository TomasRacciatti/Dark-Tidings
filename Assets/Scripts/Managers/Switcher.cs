using System;
using UnityEngine;

namespace Patterns
{
    public class Switcher : MonoBehaviour
    {
        [SerializeField] private int activeIndex = 0;

        private void Awake()
        {
            print(transform.childCount);
        }

        void Start()
        {
            SwitchTo(activeIndex);
        }

        public void SwitchTo(int index)
        {
            int childCount = transform.childCount;
            
            print(transform.childCount + " : " + index);

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

        public static int GetIndexSwitcher(Switcher switcher)
        {
            if (!switcher.gameObject.activeInHierarchy) return -1;
            return switcher.GetCurrentIndex();
        }
    }
}
