using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AddressableManager : Singleton<AddressableManager>
{
    #region 어드레서블 에셋
    Dictionary<string, UnityEngine.Object> loadedAssets = new();

    public bool IsReady { get; private set; }

    public Action completeAction;


    public IEnumerator LoadAssets(List<AssetReference> assets)
    {
        foreach (var asset in assets)
        {
            string key = asset.AssetGUID;

            // 이미 로딩돼 있으면 스킵
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
    #endregion

    #region 패치
    public IEnumerator CheckAndDownload(List<AssetReference> assets)
    {
        // 0. 카탈로그 업데이트 체크 (서버의 최신 지도 가져오기)
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;

        if (checkHandle.Result.Count > 0)
        {
            var updateHandle = Addressables.UpdateCatalogs(checkHandle.Result, false);
            yield return updateHandle;
        }

        // 리스트에 있는 모든 에셋이 포함된 번들의 총 크기를 계산
        var sizeHandle = Addressables.GetDownloadSizeAsync(assets);
        yield return sizeHandle;

        long totalSize = sizeHandle.Result;

        if (totalSize > 0)
        {
            Debug.Log($"총 다운로드 크기: {totalSize / 1048576.0f:F2} MB");

            // 2. 실제 다운로드 시작 (리스트 전체를 한꺼번에 전달)
            var downloadHandle = Addressables.DownloadDependenciesAsync(assets, Addressables.MergeMode.Union);

            while (!downloadHandle.IsDone)
            {
                float progress = downloadHandle.PercentComplete;
                Debug.Log($"전체 패치 진행률: {progress * 100}%");
                yield return null;
            }

            Addressables.Release(downloadHandle);
        }

        Addressables.Release(sizeHandle);

        // 3. 패치가 끝났으니 기존 로드 로직 실행
        yield return LoadAssets(assets);
    }

    #endregion
}
