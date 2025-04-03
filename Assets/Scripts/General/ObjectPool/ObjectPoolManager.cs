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

    private void InitialSpawns()
    {
        foreach (var obj in initialObjects)
        {
            PooledObjectInfo pool = new PooledObjectInfo() { lookupString = obj.prefab.name };
            objectPools.Add(pool);
            Transform folder = new GameObject(obj.prefab.name).transform;
            folder.SetParent(transform, false);
            pool.folder = folder;
            for (int i = 0; i < obj.initialSpawn; i++)
            {
                GameObject spawneableObject = Instantiate(obj.prefab, Vector3.zero, Quaternion.identity);
                pool.inactiveObjects.Enqueue(spawneableObject);
                spawneableObject.SetActive(false);
                spawneableObject.transform.SetParent(folder, false);
            }
        }
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // Find the pool corresponding to the prefab
        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == prefab.name);

        if (pool == null) // If the pool doesn't exist, create it
        {
            pool = new PooledObjectInfo() { lookupString = prefab.name };
            objectPools.Add(pool);
        }

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
            spawneableObject = Instantiate(prefab, position, rotation);
            spawneableObject.transform.SetParent(pool.folder, false);
        }

        return spawneableObject;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        string objName = obj.name.Substring(0, obj.name.Length - 7); // Remove "(Clone)" from the name

        PooledObjectInfo pool = objectPools.Find(p => p.lookupString == objName);

        if (pool == null) // If the pool doesn't exist, create it
        {
            pool = new PooledObjectInfo() { lookupString = objName };
            objectPools.Add(pool);
        }

        pool.inactiveObjects.Enqueue(obj); // Add the object back to the queue
        obj.SetActive(false);
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