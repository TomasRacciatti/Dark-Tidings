using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int _pellets = 10;
    
    public Shotgun()
    {
        weaponName = "Shotgun";
    }
    
    protected override void Shoot()
    {
        base.Shoot();
        for (int i = 0; i < _pellets; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * Mathf.Tan(15f * Mathf.Deg2Rad);
            Vector3 spreadOffset = (_firePoint.up * randomCircle.y) + (_firePoint.forward * randomCircle.x);
            Vector3 shootDirection = (_firePoint.right + spreadOffset).normalized;
            CreateRay(shootDirection);
        }
    }
}
