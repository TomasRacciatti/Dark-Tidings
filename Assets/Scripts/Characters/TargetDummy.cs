using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
