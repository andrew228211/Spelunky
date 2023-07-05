using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [System.Serializable]
    public class Pool
    {
        public string name;
        public Room prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<Room>> poolDictionary;
    private void Awake()
    {
        instance = this;
    
        poolDictionary = new Dictionary<string, Queue<Room>>();
        foreach (Pool pool in pools)
        {
            Queue<Room> objectPool = new Queue<Room>();
            for (int i = 0; i < pool.size; i++)
            {
                Room obj = Instantiate(pool.prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.name, objectPool);
        }
    }
    public Room GetRoom(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogError("Don't find" + name + "in pool");
            return null;
        }
        Room objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        poolDictionary[name].Enqueue(objectToSpawn);   
        return objectToSpawn;
    }
}
