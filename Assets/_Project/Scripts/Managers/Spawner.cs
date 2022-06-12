using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum TypePool
{
    Default_Bullets_Pool,
    Ghoul_Pool,
    Ghoul_Boss_Pool,
    Ghoul_Festering_Pool,
    Ghoul_Groresque_Pool,
    Ghoul_Scavenger_Pool
}


public class Spawner : CustomSerializedMonoBehaviour
{
    [System.Serializable]
    public struct ObjectPool
    {
        public TypePool TypePool;
        public GameObject Prefab;   //< It has to be ISpawnable
        public int SizePool;
        public Transform Parent; //< It can be null
    }

    public List<ObjectPool> Pools = new List<ObjectPool>();
    public Dictionary<TypePool, Queue<GameObject>> PoolDictionary = new Dictionary<TypePool, Queue<GameObject>>();

    private void Start()
    {
        SpawnPools();
        
    }

    public void SpawnPools()
    {
        ClearPools(); //< To avoid duplications

        foreach(var pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            int length = pool.SizePool;
            Transform parent = pool.Parent == null ? this.transform : pool.Parent;
            for (int i = 0; i < length; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, parent);
                obj.SetActive(false);

                var spawnable =  obj.GetComponentInChildren<ISpawnable>();
                spawnable.MyPool = pool.TypePool;

                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.TypePool, objectPool);
        }
    }

    public void ClearPools() 
    { 
        foreach(var pool in PoolDictionary)
        {
            int length = pool.Value.Count;
            for (int i = 0; i < length; i++)
            {
                DestroyImmediate(pool.Value.Dequeue());
            }
        }
        PoolDictionary.Clear();
    }

    private GameObject CreateToPool(TypePool type)
    {
        foreach(var pool in Pools)
        {
            if(pool.TypePool == type)
            {
                Transform parent = pool.Parent == null ? this.transform : pool.Parent;
                GameObject obj = Instantiate(pool.Prefab, parent);
                obj.SetActive(false);

                var spawnable = obj.GetComponentInChildren<ISpawnable>();
                spawnable.MyPool = pool.TypePool;
                return obj;
            }
        }
        return null;
    }

    public GameObject GetFromPool(TypePool type)
    {
        if (!PoolDictionary.ContainsKey(type)) return null; //< We will have a error
        GameObject obj;
        if (PoolDictionary[type].Count == 0) obj = CreateToPool(type);
        else obj = PoolDictionary[type].Dequeue();
        return obj;
    }

    public void ReturnToPool(TypePool type, GameObject obj)
    {
        obj.SetActive(false);
        PoolDictionary[type].Enqueue(obj);
    }
}
