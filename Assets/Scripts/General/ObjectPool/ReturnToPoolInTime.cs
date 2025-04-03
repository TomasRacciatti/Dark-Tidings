using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolInTime : MonoBehaviour
{
    [SerializeField] public float time = 5;

    private void OnEnable()
    {
        Invoke("ReturnToPool", time);
    }

    protected virtual void ReturnToPool()
    {
        ObjectPoolManager.instance.ReturnObjectToPool(gameObject);
    }
}
