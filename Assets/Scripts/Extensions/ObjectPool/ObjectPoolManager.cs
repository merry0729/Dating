using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public GameObject conversationPrefab;

    public Dictionary<PoolType, GameObject> poolPrefabDic = new Dictionary<PoolType, GameObject>();
    public Dictionary<PoolType, GameObject> poolBoxDic = new Dictionary<PoolType, GameObject>();
    public Dictionary<PoolType, List<PoolingObject>> poolObjectDic = new Dictionary<PoolType, List<PoolingObject>>();
    public Dictionary<PoolType, PoolParentType> poolParentDic = new Dictionary<PoolType, PoolParentType>();

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (_instance == null)
        {
            InitPool();
        }
    }

    // Pool Dictionary Setting.
    void InitPool()
    {
        // 풀링 오브젝트 프리팹.
        poolPrefabDic.Add(PoolType.Conversation, conversationPrefab);
        // 풀링 오브젝트 타입 초기화.
        poolParentDic.Add(PoolType.Conversation, PoolParentType.UI);

        for (int index = 0; index < Enum.GetValues(typeof(PoolType)).Length; index++)
        {
            // 풀링 오브젝트 박스.
            GameObject poolBox = new GameObject($"{((PoolType)index).ToString()} Pool");
            poolBox.transform.SetParent(transform);
            poolBoxDic.Add((PoolType)index, poolBox);

            // 풀링 오브젝트 리스트 초기화.
            poolObjectDic[(PoolType)index] = new List<PoolingObject>();
        }
    }

    public PoolingObject GetPooledObject(GameObject pooledObejct)
    {
        return pooledObejct.GetComponent<PoolingObject>();
    }

    // Create Pool Object.
    PoolingObject CreatePoolObject(PoolType poolType)
    {
        PoolingObject pooledObject = Instantiate(poolPrefabDic[poolType], poolBoxDic[poolType].transform).AddComponent<PoolingObject>();
        poolObjectDic[poolType].Add(pooledObject);
        return pooledObject;
    }

    // Load Pool Object.
    PoolingObject LoadPoolObject(PoolType poolType)
    {
        PoolingObject loadObject = poolObjectDic[poolType].Find(x => !x.gameObject.activeSelf);
        return loadObject;
    }

    // Enable Pool Object.
    public void EnablePoolObject(PoolType poolType, PoolingObject poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolObject.transform.SetParent(poolBoxDic[poolType].transform);
    }

    // Destroy Pool Object.
    void DestroyPoolObject(PoolType poolType, PoolingObject poolObject)
    {
        poolObjectDic[poolType].Remove(poolObject);
        Destroy(poolObject.gameObject);
    }

    // Pool Object.
    public PoolingObject PoolObject(PoolType poolType)
    {
        PoolingObject selectedPoolingObject = null;

        if (poolObjectDic[poolType].Count < 1)
            selectedPoolingObject = CreatePoolObject(poolType);
        else
        {
            selectedPoolingObject = LoadPoolObject(poolType);

            if (selectedPoolingObject == null)
                selectedPoolingObject = CreatePoolObject(poolType);
        }

        switch (poolParentDic[poolType])
        {
            case PoolParentType.UI:
                selectedPoolingObject.transform.SetParent(UIManager.Instance.GetCurrentSceneUI().transform);
                break;

            case PoolParentType.GameObject:
                break;
        }

        selectedPoolingObject.gameObject.SetActive(true);
        return selectedPoolingObject;
    }
}
