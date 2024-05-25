using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    Conversation,
}

public enum PoolParentType
{
    UI,
    GameObject,
}

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject conversationPrefab;

    public Dictionary<PoolType, GameObject> poolPrefabDic = new Dictionary<PoolType, GameObject>();
    public Dictionary<PoolType, GameObject> poolBoxDic = new Dictionary<PoolType, GameObject>();
    public Dictionary<PoolType, List<PoolingObject>> poolObjectDic = new Dictionary<PoolType, List<PoolingObject>>();
    public Dictionary<PoolType, PoolParentType> poolParentDic = new Dictionary<PoolType, PoolParentType>();

    public PoolingObject currentPoolingObject;

    private void Awake()
    {
        InitPool();
    }

    void InitPool()
    {
        Debug.Log($"Enum.GetValues(typeof(PoolType)).Length = {Enum.GetValues(typeof(PoolType)).Length}");

        for (int index = 0; index < Enum.GetValues(typeof(PoolType)).Length; index++)
        {
            Debug.Log($"index = {index}");

            // 풀링 오브젝트 프리팹.
            poolPrefabDic.Add((PoolType)index, conversationPrefab);

            // 풀링 오브젝트 박스.
            GameObject poolBox = new GameObject($"{((PoolType)index).ToString()} Pool");
            poolBox.transform.SetParent(transform);
            poolBoxDic.Add((PoolType)index, poolBox);

            // 풀링 오브젝트 리스트 초기화.
            poolObjectDic[(PoolType)index] = new List<PoolingObject>();
        }

        // 풀링 오브젝트의 부모 초기화.
        poolParentDic.Add(PoolType.Conversation, PoolParentType.UI);
    }

    public PoolingObject GetPooledObject(GameObject pooledObejct)
    {
        return pooledObejct.GetComponent<PoolingObject>();
    }

    PoolingObject CreatePoolObject(PoolType poolType)
    {
        PoolingObject pooledObject = Instantiate(poolPrefabDic[poolType], poolBoxDic[poolType].transform).GetComponent<PoolingObject>();
        poolObjectDic[poolType].Add(pooledObject);
        return pooledObject;
    }

    PoolingObject LoadPoolObject(PoolType poolType)
    {
        PoolingObject loadObject = poolObjectDic[poolType].Find(x => !x.gameObject.activeSelf);
        return loadObject;
    }

    void DestroyPoolObject(PoolType poolType)
    {
        
    }

    public void PoolObject(PoolType poolType)
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
                selectedPoolingObject.transform.parent = UIManager.Instance.GetCurrentSceneUI().transform;
                break;

            case PoolParentType.GameObject:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PoolObject(PoolType.Conversation);
    }
}
