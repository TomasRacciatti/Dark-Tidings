using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock : Weapon
{
    public Glock()
    {
        weaponName = "Glock";
    }
    
    protected override void Shoot()
    {
        base.Shoot();
        CreateRay();
    }
}
