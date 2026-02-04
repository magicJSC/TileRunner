using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{

    Dictionary<string, UnityEngine.Object> loadedAssets = new();

    public bool IsReady { get; private set; }

    public Action completeAction;


    public IEnumerator LoadAssets(List<AssetReference> assets)
    {
        foreach (var asset in assets)
        {
            string key = asset.AssetGUID;

            // ¿ÃπÃ ∑Œµ˘µ≈ ¿÷¿∏∏È Ω∫≈µ
            if (loadedAssets.ContainsKey(key))
                continue;

            AsyncOperationHandle<UnityEngine.Object> handle =
                asset.LoadAssetAsync<UnityEngine.Object>();

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                loadedAssets.Add(key, handle.Result);
            }
            else
            {
                Debug.LogError($"[AddressableManager] Load failed : {key}");
            }
        }

        IsReady = true;
        completeAction?.Invoke();
    }

    public T Get<T>(string key) where T : UnityEngine.Object
    {
        if (!loadedAssets.TryGetValue(key, out var obj))
        {
            Debug.LogError($"[AddressableManager] Asset not loaded : {key}");
            return null;
        }

        return obj as T;
    }

    public T Get<T>(AssetReference reference) where T : UnityEngine.Object
    {
        if (reference == null)
            return null;

        string key = reference.AssetGUID;

        if (!loadedAssets.TryGetValue(key, out var obj))
        {
            Debug.LogError($"[AddressableManager] Asset not loaded : {reference.RuntimeKey}");
            return null;
        }

        return obj as T;
    }
}
