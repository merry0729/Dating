// Auto Generated Code By JsonTableGenerator.
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public partial class TableLoader
{
    public async UniTask<bool> LoadHeavySize(bool delayLoad)
    {
#if LIVE_BUILD        
        _tableLoadingCount = 0;
        _loadedTable.Clear();
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public async UniTask<bool> LoadNormalSize(bool delayLoad)
    {
#if LIVE_BUILD        
        _tableLoadingCount = 0;
        _loadedTable.Clear();
        LoadTableGenericClient<ConversationData>(delayLoad);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<ConversationData>(delayLoad);
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public async UniTask<bool> LoadDependanciesOnly(bool delayLoad)
    {
#if LIVE_BUILD        
        _tableLoadingCount = 0;
        _loadedTable.Clear();
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public async UniTask<bool> LoadAllWithoutDependancies(bool delayLoad)
    {
#if LIVE_BUILD        
        _tableLoadingCount = 0;
        _loadedTable.Clear();
        LoadTableGenericClient<ConversationData>(delayLoad);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<ConversationData>(delayLoad);
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public async UniTask<bool> LoadAll(bool delayLoad)
    {
#if LIVE_BUILD        
        _tableLoadingCount = 0;
        _loadedTable.Clear();
        LoadTableGenericClient<ConversationData>(delayLoad);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<ConversationData>(delayLoad);
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public async UniTask<bool> LoadAllCompletion(bool delayLoad)
    {
#if LIVE_BUILD        
        _tableLoadingCount = 0;
        _loadedTable.Clear();
        LoadTableGenericClient<ConversationData>(delayLoad, true);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<ConversationData>(delayLoad, true);
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public async UniTask<bool> LoadForServer(bool delayLoad)
    {
#if LIVE_BUILD        
        _loadedTable.Clear();
        
        await LoadTableGenericServer<ConversationData>(delayLoad);
        
        OnPostLoad();
        return true;
#else        
        try {
            _loadedTable.Clear();
            
            await LoadTableGenericServer<ConversationData>(delayLoad);
            
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }

    public bool LoadOverrides(bool delayLoad)
    {
#if LIVE_BUILD        
        _loadedTable.Clear();
        
        LoadTableGenericOverride<ConversationData>(delayLoad);
        
        OnPostLoad();
        return true;
#else        
        try {
            _loadedTable.Clear();
            
            LoadTableGenericOverride<ConversationData>(delayLoad);
            
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }
}
