using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : Singleton<GameManager>
{
    [Header("[ Manager ]")]
    public GameObject uiManagerPrefab;
    public GameObject sceneControlManagerPrefab;
    public GameObject objectPoolManagerPrefab;
    public GameObject conversationManagerPrefab;
    public GameObject PhoneManagerPrefab;
    public GameObject soundManagerPrefab;
    public GameObject settingManagerPrefab;

    [Header("[ SaveData ]")]
    public GameData gameData;

    private void Awake()
    {
        TableLoader.OnTableLoadComplete += TableLoadComplete;
        if (_instance == null)
        {
            LoadTablesAll();
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    void TableLoadComplete()
    {
        Debug.Log($"TableLoader.OnTableLoadComplete");

        Instantiate(uiManagerPrefab, transform);
        Instantiate(sceneControlManagerPrefab, transform);
        Instantiate(objectPoolManagerPrefab, transform);
        Instantiate(conversationManagerPrefab, transform);
        Instantiate(PhoneManagerPrefab, transform);
        Instantiate(soundManagerPrefab, transform);
        Instantiate(settingManagerPrefab, transform);
   
    }

    #region [ Json Data Table Load ]

    public static bool IsTableAllLoaded { get; private set; } = false;
    public static bool IsTableDependancyLoaded { get; private set; } = false;
    public static bool IsTableNormalSizeLoaded { get; private set; } = false;
    public static bool IsTableHeavySizeLoaded { get; private set; } = false;

    public static async UniTask LoadTablesAll()
    {
        if (IsTableAllLoaded)
            return;

        // Table Load.
        var tableLoader = new TableLoader();
        //if (CommonUtils.IsServerBuild())
        //    await tableLoader.LoadForServer(true);
        //else
        //{
        if (false == IsTableDependancyLoaded && false == IsTableNormalSizeLoaded && false == IsTableHeavySizeLoaded)
            await tableLoader.LoadAll(false);
        else
        {
            if (false == IsTableDependancyLoaded)
                await tableLoader.LoadDependanciesOnly(false);
            if (false == IsTableNormalSizeLoaded)
                await tableLoader.LoadNormalSize(false);
            if (false == IsTableHeavySizeLoaded)
                await tableLoader.LoadHeavySize(false);
        }
        //}
        //await tableLoader.Load(false);

        tableLoader.LoadOverrides(false);

        IsTableAllLoaded = true;
        IsTableDependancyLoaded = true;
        IsTableNormalSizeLoaded = true;
        IsTableHeavySizeLoaded = true;
    }

    #endregion
}
