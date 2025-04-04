using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<Spawn> initialObjects;
    public List<PooledObjectInfo> objectPools = new List<PooledObjectInfo>();
    public static ObjectPoolManager instance;

    [Serializable]
    private struct Spawn
    {
        public int initialSpawn;
        public GameObject prefab;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitialSpawns();
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

    private GameObject NewObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject spawneableObject = Instantiate(prefab, position, rotation);
        PooledObjectInfo pool = GetPool(prefab.name);
        pool.inactiveObjects.Enqueue(spawneableObject);
        spawneableObject.SetActive(false);
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

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // Find the pool corresponding to the prefab
        PooledObjectInfo pool = GetPool(prefab.name);
        
        GameObject spawneableObject;

        if (pool.inactiveObjects.Count > 0) // If the pool has inactive objects
        {
            spawneableObject = pool.inactiveObjects.Dequeue(); // Get the first object in the queue
            spawneableObject.transform.position = position;
            spawneableObject.transform.rotation = rotation;
            spawneableObject.SetActive(true);
        }
        else // If no inactive objects exist, create a new one
        {
            spawneableObject = NewObject(prefab, position, rotation);
        }
        return spawneableObject;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        string objName = obj.name.Replace("(Clone)", "").Trim();

        PooledObjectInfo pool = GetPool(objName);

        obj.SetActive(false);
        pool.inactiveObjects.Enqueue(obj); // Add the object back to the queue
    }

    public void ReturnObjectToPool(GameObject obj, float delay)
    {
        StartCoroutine(ReturnObjectWithDelay(obj, delay));
    }

    private IEnumerator ReturnObjectWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnObjectToPool(obj);
    }
}