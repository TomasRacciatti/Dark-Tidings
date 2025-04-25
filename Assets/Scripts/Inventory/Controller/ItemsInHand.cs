using UnityEngine;

namespace Inventory.Controller
{
    public class ItemsInHand : MonoBehaviour
    {
        public static ItemsInHand Instance { get; private set; }
    
        public Item glock;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Shoot()
        {
            if (!glock.gameObject.activeSelf) return;
            glock.StartUsing();
        }

        public void ActivateGlock()
        {
            glock.gameObject.SetActive(true);
        }
    }
}
