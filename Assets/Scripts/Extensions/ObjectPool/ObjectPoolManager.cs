using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    /// <summary>
    /// Ǯ���� ������Ʈ ������ (����� 1��)
    /// </summary>
    public GameObject conversationPrefab;
    public GameObject messagePrefab_Sender;
    public GameObject messagePrefab_Mine;

    /// <summary>
    /// Ǯ�� ������Ʈ�� ���� �� Dictionary ����.
    /// 
    /// poolPrefabDic : ������ Dictionary
    /// poolBoxDic : ��Ȱ��ȭ�� ������Ʈ�� ��Ƴ��� ������Ʈ Dictionary
    /// poolObjectDic : Ǯ�� ������Ʈ Dictionary
    /// poolParentDic : Ǯ���� ������Ʈ���� Parent Ÿ�� Dictionary
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
    /// [Pool ���� Dictionary �ʱ�ȭ]
    /// </summary>
    void InitPool()
    {
        // Ǯ�� ������Ʈ ������ Dictionary ���.
        poolPrefabDic.Add(PoolType.Conversation, conversationPrefab);
        poolPrefabDic.Add(PoolType.Message_Mine, messagePrefab_Mine);
        poolPrefabDic.Add(PoolType.Message_Sender, messagePrefab_Sender);
        // Ǯ�� ������Ʈ Ÿ�� Dictionart ���.
        poolParentDic.Add(PoolType.Conversation, PoolParentType.UI);
        poolParentDic.Add(PoolType.Message_Mine, PoolParentType.UI);
        poolParentDic.Add(PoolType.Message_Sender, PoolParentType.UI);

        // PoolType�� Length�� ���� ���� �� �ʱ�ȭ.
        for (int index = 0; index < Enum.GetValues(typeof(PoolType)).Length; index++)
        {
            // Ǯ�� ������Ʈ �ڽ�.
            GameObject poolBox = new GameObject($"{((PoolType)index).ToString()} Pool");
            poolBox.transform.SetParent(transform);
            poolBoxDic.Add((PoolType)index, poolBox);

            // Ǯ�� ������Ʈ ����Ʈ �ʱ�ȭ.
            poolObjectDic[(PoolType)index] = new List<PoolingObject>();
        }
    }

    /// <summary>
    /// [Ư�� ������Ʈ�� Pooling ���� Get]
    /// </summary>
    /// <param name="pooledObejct"></param>
    /// <returns></returns>
    public PoolingObject GetPooledObject(GameObject pooledObejct)
    {
        return pooledObejct.GetComponent<PoolingObject>();
    }


    /// <summary>
    /// [PoolType�� ���� Ǯ�� ������Ʈ ����]
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
    /// [������ ������Ʈ �߿� ��������� ���� ������Ʈ �ε�]
    /// </summary>
    /// <param name="poolType"></param>
    /// <returns></returns>
    PoolingObject LoadPoolObject(PoolType poolType)
    {
        PoolingObject loadObject = poolObjectDic[poolType].Find(x => !x.gameObject.activeSelf);
        return loadObject;
    }

    /// <summary>
    /// [����� ������Ʈ ��Ȱ��ȭ]
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
    /// [������Ʈ ����]
    /// </summary>
    /// <param name="poolType"></param>
    /// <param name="poolObject"></param>
    void DestroyPoolObject(PoolType poolType, PoolingObject poolObject)
    {
        poolObjectDic[poolType].Remove(poolObject);
        Destroy(poolObject.gameObject);
    }

    /// <summary>
    /// [������Ʈ Ǯ��]
    /// ������ ������Ʈ�� ���ų� ��� ������̸� CreatePoolObject�� ȣ���ϰ�, ��� ������ ������Ʈ�� �ִٸ� LoadPoolObject ȣ���Ѵ�.
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
