using System;
using System.Collections;
using System.Collections.Generic;
using Managers.ObjectPool;
using UnityEngine;

namespace Patterns.ObjectPool
{
    public class ObjectPoolManager : MonoBehaviour
    {
        [SerializeField] private List<Spawn> initialObjects;
        public List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();
        public static ObjectPoolManager Instance;

        [Serializable]
        private struct Spawn
        {
            public int initialSpawn;
            public GameObject prefab;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //InitialSpawns();
            StartCoroutine(InitialSpawnsAsync());
        }

        private PooledObjectInfo NewPool(string poolName)
        {
            PooledObjectInfo pool = new PooledObjectInfo() { lookupString = poolName };
            objectPools.Add(pool);
            GameObject folder = new GameObject(poolName);
            folder.transform.SetParent(transform, false);
            pool.folder = folder.transform;
            return pool;
        }

        private PooledObjectInfo GetPool(string poolName)
        {
            PooledObjectInfo pool = objectPools.Find(x => x.lookupString == poolName);
            if (pool == null) // If the pool doesn't exist, create it
            {
                pool = NewPool(poolName);
            }
            return pool;
        }

        private GameObject NewObject(GameObject prefab, Vector3 position, Quaternion rotation, bool enqueue = true)
        {
            GameObject spawneableObject = Instantiate(prefab, position, rotation);
            PooledObjectInfo pool = GetPool(prefab.name);
            if (enqueue)
            {
                pool.inactiveObjects.Enqueue(spawneableObject);
                spawneableObject.SetActive(false);
            }
            spawneableObject.transform.SetParent(pool.folder.transform, false);
            return spawneableObject;
        }

        private void InitialSpawns()
        {
            foreach (var obj in initialObjects)
            {
                for (int i = 0; i < obj.initialSpawn; i++)
                {
                    NewObject(obj.prefab, Vector3.zero, Quaternion.identity);
                }
            }
        }

        private IEnumerator InitialSpawnsAsync()
        {
            const float maxFrameTime = 0.005f;

            var stopwatch = new System.Diagnostics.Stopwatch();

            foreach (var obj in initialObjects)
            {
                stopwatch.Reset();
                stopwatch.Start();

                for (int i = 0; i < obj.initialSpawn; i++)
                {
                    NewObject(obj.prefab, Vector3.zero, Quaternion.identity);
                    
                    if (stopwatch.Elapsed.TotalSeconds >= maxFrameTime)
                    {
                        stopwatch.Reset();
                        yield return null;
                        stopwatch.Start();
                    }
                }

                stopwatch.Stop();
            }
        }

        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            PooledObjectInfo pool = GetPool(prefab.name);
        
            GameObject spawneableObject;

            if (pool.inactiveObjects.Count > 0)
            {
                spawneableObject = pool.inactiveObjects.Dequeue();
                spawneableObject.transform.position = position;
                spawneableObject.transform.rotation = rotation;
                spawneableObject.SetActive(true);
            }
            else
            {
                spawneableObject = NewObject(prefab, position, rotation, false);
            }
            return spawneableObject;
        }

        //Sobrecarga con tiempo de vida establecido
        public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation, float delay)
        {
            GameObject spawneableObject = SpawnObject(prefab, position, rotation);
            ReturnObjectToPool(spawneableObject, delay);
            return spawneableObject;
        }

        public void ReturnObjectToPool(GameObject obj)
        {
            string objName = obj.name.Replace("(Clone)", "").Trim();

            PooledObjectInfo pool = GetPool(objName);
            obj.transform.SetParent(pool.folder.transform, false);
            
            obj.SetActive(false);
            pool.inactiveObjects.Enqueue(obj);
        }

        //Sobrecarga con tiempo de vida establecido
        public void ReturnObjectToPool(GameObject obj, float delay)
        {
            Coroutine routine = StartCoroutine(ReturnObjectWithDelay(obj, delay));
        }

        private IEnumerator ReturnObjectWithDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnObjectToPool(obj);
        }
    }
}