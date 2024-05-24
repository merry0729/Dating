using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Conversation,

}

public class ObjectPool : MonoBehaviour
{
    public Dictionary<PoolType, GameObject> poolBoxDic = new Dictionary<PoolType, GameObject>();
    public Dictionary<PoolType, List<PoolingObject>> poolObjectDic = new Dictionary<PoolType, List<PoolingObject>>();

    public PoolingObject currentPoolingObject;

    private void Awake()
    {
        InitPool();
    }

    void InitPool()
    {
        for(int index = 0; index < Enum.GetValues(typeof(PoolType)).Length; index++)
        {
            //poolDic.Add((PoolType)index, )

            GameObject poolBox = Instantiate(new GameObject(), transform);
            poolBox.name = $"{((PoolType)index).ToString()} Pool";
            poolBoxDic.Add((PoolType)index, poolBox);
        }
    }

    public PoolingObject GetPooledObject(GameObject pooledObejct)
    {
        return pooledObejct.GetComponent<PoolingObject>();
    }

    void CreatePoolObject(PoolType poolType)
    { 
        
    }

    void LoadPoolObject(PoolType poolType)
    {

    }

    void DestroyPoolObject(PoolType poolType)
    {
        
    }

    public void PoolObject(PoolType poolType)
    {
        if (poolObjectDic[poolType].Count < 1)
            CreatePoolObject(poolType);
        else
        {
            if (poolObjectDic[poolType].Find(x => x.gameObject.activeSelf == false))
                LoadPoolObject(poolType);
            else
                CreatePoolObject(poolType);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PoolObject(PoolType.Conversation);
    }
}
