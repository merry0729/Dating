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
        LoadTableGenericClient<CharacterSettingData>(delayLoad);
        LoadTableGenericClient<ConversationData>(delayLoad);
        LoadTableGenericClient<ConversationSettingData>(delayLoad);
        LoadTableGenericClient<MessengerData>(delayLoad);
        LoadTableGenericClient<OptionsData>(delayLoad);
        await UniTask.Delay(20);
        LoadTableGenericClient<SettingData>(delayLoad);
        LoadTableGenericClient<SoundData>(delayLoad);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<CharacterSettingData>(delayLoad);
            LoadTableGenericClient<ConversationData>(delayLoad);
            LoadTableGenericClient<ConversationSettingData>(delayLoad);
            LoadTableGenericClient<MessengerData>(delayLoad);
            LoadTableGenericClient<OptionsData>(delayLoad);
            await UniTask.Delay(20);
            LoadTableGenericClient<SettingData>(delayLoad);
            LoadTableGenericClient<SoundData>(delayLoad);
            
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
        LoadTableGenericClient<CharacterSettingData>(delayLoad);
        LoadTableGenericClient<ConversationData>(delayLoad);
        LoadTableGenericClient<ConversationSettingData>(delayLoad);
        LoadTableGenericClient<MessengerData>(delayLoad);
        LoadTableGenericClient<OptionsData>(delayLoad);
        await UniTask.Delay(20);
        LoadTableGenericClient<SettingData>(delayLoad);
        LoadTableGenericClient<SoundData>(delayLoad);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<CharacterSettingData>(delayLoad);
            LoadTableGenericClient<ConversationData>(delayLoad);
            LoadTableGenericClient<ConversationSettingData>(delayLoad);
            LoadTableGenericClient<MessengerData>(delayLoad);
            LoadTableGenericClient<OptionsData>(delayLoad);
            await UniTask.Delay(20);
            LoadTableGenericClient<SettingData>(delayLoad);
            LoadTableGenericClient<SoundData>(delayLoad);
            
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
        LoadTableGenericClient<CharacterSettingData>(delayLoad);
        LoadTableGenericClient<ConversationData>(delayLoad);
        LoadTableGenericClient<ConversationSettingData>(delayLoad);
        LoadTableGenericClient<MessengerData>(delayLoad);
        LoadTableGenericClient<OptionsData>(delayLoad);
        await UniTask.Delay(20);
        LoadTableGenericClient<SettingData>(delayLoad);
        LoadTableGenericClient<SoundData>(delayLoad);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<CharacterSettingData>(delayLoad);
            LoadTableGenericClient<ConversationData>(delayLoad);
            LoadTableGenericClient<ConversationSettingData>(delayLoad);
            LoadTableGenericClient<MessengerData>(delayLoad);
            LoadTableGenericClient<OptionsData>(delayLoad);
            await UniTask.Delay(20);
            LoadTableGenericClient<SettingData>(delayLoad);
            LoadTableGenericClient<SoundData>(delayLoad);
            
            await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
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
        LoadTableGenericClient<CharacterSettingData>(delayLoad, true);
        LoadTableGenericClient<ConversationData>(delayLoad, true);
        LoadTableGenericClient<ConversationSettingData>(delayLoad, true);
        LoadTableGenericClient<MessengerData>(delayLoad, true);
        LoadTableGenericClient<OptionsData>(delayLoad, true);
        await UniTask.Delay(20);
        LoadTableGenericClient<SettingData>(delayLoad, true);
        LoadTableGenericClient<SoundData>(delayLoad, true);
        
        await UniTask.WaitUntil(() => 0 == _tableLoadingCount);
        OnPostLoad();
        return true;
#else        
        try {
            _tableLoadingCount = 0;
            _loadedTable.Clear();
            LoadTableGenericClient<CharacterSettingData>(delayLoad, true);
            LoadTableGenericClient<ConversationData>(delayLoad, true);
            LoadTableGenericClient<ConversationSettingData>(delayLoad, true);
            LoadTableGenericClient<MessengerData>(delayLoad, true);
            LoadTableGenericClient<OptionsData>(delayLoad, true);
            await UniTask.Delay(20);
            LoadTableGenericClient<SettingData>(delayLoad, true);
            LoadTableGenericClient<SoundData>(delayLoad, true);
            
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
        
        await LoadTableGenericServer<CharacterSettingData>(delayLoad);
        await LoadTableGenericServer<ConversationData>(delayLoad);
        await LoadTableGenericServer<ConversationSettingData>(delayLoad);
        await LoadTableGenericServer<MessengerData>(delayLoad);
        await LoadTableGenericServer<OptionsData>(delayLoad);
        await LoadTableGenericServer<SettingData>(delayLoad);
        await LoadTableGenericServer<SoundData>(delayLoad);
        
        OnPostLoad();
        return true;
#else        
        try {
            _loadedTable.Clear();
            
            await LoadTableGenericServer<CharacterSettingData>(delayLoad);
            await LoadTableGenericServer<ConversationData>(delayLoad);
            await LoadTableGenericServer<ConversationSettingData>(delayLoad);
            await LoadTableGenericServer<MessengerData>(delayLoad);
            await LoadTableGenericServer<OptionsData>(delayLoad);
            await LoadTableGenericServer<SettingData>(delayLoad);
            await LoadTableGenericServer<SoundData>(delayLoad);
            
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
        
        LoadTableGenericOverride<CharacterSettingData>(delayLoad);
        LoadTableGenericOverride<ConversationData>(delayLoad);
        LoadTableGenericOverride<ConversationSettingData>(delayLoad);
        LoadTableGenericOverride<MessengerData>(delayLoad);
        LoadTableGenericOverride<OptionsData>(delayLoad);
        LoadTableGenericOverride<SettingData>(delayLoad);
        LoadTableGenericOverride<SoundData>(delayLoad);
        
        OnPostLoad();
        return true;
#else        
        try {
            _loadedTable.Clear();
            
            LoadTableGenericOverride<CharacterSettingData>(delayLoad);
            LoadTableGenericOverride<ConversationData>(delayLoad);
            LoadTableGenericOverride<ConversationSettingData>(delayLoad);
            LoadTableGenericOverride<MessengerData>(delayLoad);
            LoadTableGenericOverride<OptionsData>(delayLoad);
            LoadTableGenericOverride<SettingData>(delayLoad);
            LoadTableGenericOverride<SoundData>(delayLoad);
            
            OnPostLoad();
            return true;
        } catch (Exception e) {
            Debug.LogError($"TableLoader get exception {e}");
            return false;
        }
#endif        
    }
}
