using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    /// <summary>
    /// 풀링할 오브젝트 프리팹 (현재는 1개)
    /// </summary>
    public GameObject conversationPrefab;
    public GameObject messagePrefab_Sender;
    public GameObject messagePrefab_Mine;

    /// <summary>
    /// 풀링 오브젝트를 관리 할 Dictionary 모음.
    /// 
    /// poolPrefabDic : 프리팹 Dictionary
    /// poolBoxDic : 비활성화된 오브젝트를 모아놓는 오브젝트 Dictionary
    /// poolObjectDic : 풀링 오브젝트 Dictionary
    /// poolParentDic : 풀링될 오브젝트들의 Parent 타입 Dictionary
    /// </summary>
    public SerializableDictionary<PoolType, GameObject> poolPrefabDic = new SerializableDictionary<PoolType, GameObject>();
    public SerializableDictionary<PoolType, GameObject> poolBoxDic = new SerializableDictionary<PoolType, GameObject>();
    public SerializableDictionary<PoolType, List<PoolingObject>> poolObjectDic = new SerializableDictionary<PoolType, List<PoolingObject>>();
    public SerializableDictionary<PoolType, PoolParentType> poolParentDic = new SerializableDictionary<PoolType, PoolParentType>();

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

    /// <summary>
    /// [Pool 관련 Dictionary 초기화]
    /// </summary>
    void InitPool()
    {
        // 풀링 오브젝트 프리팹 Dictionary 등록.
        poolPrefabDic.Add(PoolType.Conversation, conversationPrefab);
        poolPrefabDic.Add(PoolType.Message_Mine, messagePrefab_Mine);
        poolPrefabDic.Add(PoolType.Message_Sender, messagePrefab_Sender);
        // 풀링 오브젝트 타입 Dictionart 등록.
        poolParentDic.Add(PoolType.Conversation, PoolParentType.UI);
        poolParentDic.Add(PoolType.Message_Mine, PoolParentType.UI);
        poolParentDic.Add(PoolType.Message_Sender, PoolParentType.UI);

        // PoolType의 Length에 따라 생성 및 초기화.
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

    /// <summary>
    /// [특정 오브젝트의 Pooling 정보 Get]
    /// </summary>
    /// <param name="pooledObejct"></param>
    /// <returns></returns>
    public PoolingObject GetPooledObject(GameObject pooledObejct)
    {
        return pooledObejct.GetComponent<PoolingObject>();
    }


    /// <summary>
    /// [PoolType에 따른 풀링 오브젝트 생성]
    /// </summary>
    /// <param name="poolType"></param>
    /// <returns></returns>
    PoolingObject CreatePoolObject(PoolType poolType)
    {
        PoolingObject pooledObject = Instantiate(poolPrefabDic[poolType], poolBoxDic[poolType].transform).AddComponent<PoolingObject>();
        poolObjectDic[poolType].Add(pooledObject);
        return pooledObject;
    }

    /// <summary>
    /// [생성된 오브젝트 중에 사용중이지 않은 오브젝트 로드]
    /// </summary>
    /// <param name="poolType"></param>
    /// <returns></returns>
    PoolingObject LoadPoolObject(PoolType poolType)
    {
        PoolingObject loadObject = poolObjectDic[poolType].Find(x => !x.gameObject.activeSelf);
        return loadObject;
    }

    /// <summary>
    /// [사용한 오브젝트 비활성화]
    /// </summary>
    /// <param name="poolType"></param>
    /// <param name="poolObject"></param>
    public void EnablePoolObject(PoolType poolType, PoolingObject poolObject)
    {
        poolObject.gameObject.SetActive(false);
        poolObject.transform.SetParent(poolBoxDic[poolType].transform);
    }

    public void EnableAllPoolObject(PoolType poolType)
    {
        Debug.Log($"poolObjectDic[poolType] : {poolObjectDic[poolType].Count}");

        foreach(var obj in poolObjectDic[poolType])
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(poolBoxDic[poolType].transform);
        }
    }

    /// <summary>
    /// [오브젝트 삭제]
    /// </summary>
    /// <param name="poolType"></param>
    /// <param name="poolObject"></param>
    void DestroyPoolObject(PoolType poolType, PoolingObject poolObject)
    {
        poolObjectDic[poolType].Remove(poolObject);
        Destroy(poolObject.gameObject);
    }

    /// <summary>
    /// [오브젝트 풀링]
    /// 생성된 오브젝트가 없거나 모두 사용중이면 CreatePoolObject를 호출하고, 사용 가능한 오브젝트가 있다면 LoadPoolObject 호출한다.
    /// </summary>
    /// <param name="poolType"></param>
    /// <returns></returns>
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
