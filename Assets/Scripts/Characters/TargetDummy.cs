using Patterns.ObjectPool;
using TMPro;
using UnityEngine;

namespace Characters
{
    public class TargetDummy : Character
    {
        [SerializeField] private GameObject _indicator;
    
        public override void TakeDamage(int damage)
        {
            GameObject obj = ObjectPoolManager.instance.SpawnObject(_indicator, transform.position, transform.rotation, 5);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        }
    
        protected override void Death()
        {
            //cannot death
        }
    }
}
