using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool{
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static BulletPool instance;

    public List<Pool> pools;
    private Dictionary<string,Queue<GameObject>> poolDictionary;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject Fire(string tag,Vector3 position,Quaternion rotation)
    {
        Queue<GameObject> objPool = poolDictionary[tag];
        GameObject obj = objPool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void BackToPool(string tag,GameObject obj)
    {
        obj.SetActive(false);
        Queue<GameObject> objPool = poolDictionary[tag];
        objPool.Enqueue(obj);
    }

}
