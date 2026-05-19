using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject prefab;
    public int startingAmount = 20;
    public bool canExpand = true;

    private Queue<GameObject> pooledObjects = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < startingAmount; i++)
        {
            CreateNewObject();
        }
    }

    GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);

        PooledObject pooledObject = obj.GetComponent<PooledObject>();
        if (pooledObject == null)
        {
            pooledObject = obj.AddComponent<PooledObject>();
        }

        pooledObject.originPool = this;

        pooledObjects.Enqueue(obj);
        return obj;
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (pooledObjects.Count == 0)
        {
            if (canExpand)
            {
                CreateNewObject();
            }
            else
            {
                return null;
            }
        }

        GameObject obj = pooledObjects.Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pooledObjects.Enqueue(obj);
    }
}
