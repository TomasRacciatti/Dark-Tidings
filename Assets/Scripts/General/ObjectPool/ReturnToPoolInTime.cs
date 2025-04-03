using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolInTime : MonoBehaviour
{
    [SerializeField] private float _time = 5;

    private void OnEnable()
    {
        Invoke("ReturnToPool", _time);
    }

    protected virtual void ReturnToPool()
    {
        ObjectPoolManager.instance.ReturnObjectToPool(gameObject);
    }
}
