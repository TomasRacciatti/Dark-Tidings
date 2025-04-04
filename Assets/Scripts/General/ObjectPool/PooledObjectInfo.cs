using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectInfo
{
    public string lookupString;
    public Transform folder;
    public Queue<GameObject> inactiveObjects = new Queue<GameObject>();
}