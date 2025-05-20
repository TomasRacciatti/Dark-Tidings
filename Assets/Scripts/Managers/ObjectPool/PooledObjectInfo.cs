using System.Collections.Generic;
using UnityEngine;

namespace Managers.ObjectPool
{
    public class PooledObjectInfo
    {
        public string lookupString;
        public Transform folder;
        public Queue<GameObject> inactiveObjects = new Queue<GameObject>();
    }
}