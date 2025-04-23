using System.Collections.Generic;
using UnityEngine;

namespace Patterns.ObjectPool
{
    public class PooledObjectInfo
    {
        public string lookupString;
        public Transform folder;
        public Queue<GameObject> inactiveObjects = new Queue<GameObject>();
    }
}