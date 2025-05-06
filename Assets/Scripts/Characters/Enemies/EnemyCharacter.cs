using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class EnemyCharacter : Character
{
    protected override void Death()
    {
        gameObject.SetActive(false);
    }
}
