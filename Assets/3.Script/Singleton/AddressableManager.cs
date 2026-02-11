using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

        Debug.Log("로드 끝");
        IsReady = true;
        completeAction?.Invoke();
    }

    public IEnumerator LoadAssetsByLabel(string label)
    {
        // 1. 해당 라벨을 가진 모든 에셋 로드 시도
        // <UnityEngine.Object>로 로드하면 프리팹, 텍스처, 사운드 등 모두 가져올 수 있습니다.
        AsyncOperationHandle<IList<UnityEngine.Object>> handle =
            Addressables.LoadAssetsAsync<UnityEngine.Object>(label, (asset) =>
            {
                // 각 에셋이 로드될 때마다 실행되는 콜백 (선택 사항)
                // 여기에서 개별 에셋의 로드 완료 처리를 할 수 있습니다.
            });

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 2. 로드된 결과물들을 loadedAssets 딕셔너리에 저장
            IList<UnityEngine.Object> results = handle.Result;

            foreach (var obj in results)
            {
                // 어드레서블 내부 이름(Address)을 키로 사용하거나, 
                // 프로젝트 규칙에 따라 이름을 키로 사용할 수 있습니다.
                if (!loadedAssets.ContainsKey(obj.name))
                {
                    loadedAssets.Add(obj.name, obj);
                }
            }

            Debug.Log($"[AddressableManager] 라벨 '{label}' 로드 완료! (총 {results.Count}개)");
        }
        else
        {
            Debug.LogError($"[AddressableManager] 라벨 '{label}' 로드 실패!");
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
            var downloadHandle = Addressables.DownloadDependenciesAsync(assets, Addressables.MergeMode.Union);

            while (!downloadHandle.IsDone)
            {
                // 0.0 ~ 1.0 사이의 값을 보기 좋게 출력
                Debug.Log($"다운로드 중... {downloadHandle.PercentComplete * 100:F1}%");
                yield return null;
            }

            // [중요] 성공 여부 확인 추가
            if (downloadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("### 모든 에셋 다운로드 및 설치 완료! ###");
            }
            else
            {
                Debug.LogError("다운로드 중 오류가 발생했습니다.");
            }

            Addressables.Release(downloadHandle);
        }
        else
        {
            Debug.Log("업데이트할 에셋이 없습니다.");
        }

        Addressables.Release(sizeHandle);

        // 3. 패치가 끝났으니 기존 로드 로직 실행
        Debug.Log("### LoadAssets 진입 직전 ###"); // 여기에 로그를 찍어보세요!
        yield return LoadAssets(assets);
    }

    // List<AssetReference> 대신 string(라벨 이름)을 받도록 변경
    public IEnumerator CheckAndDownloadAll(string label)
    {
        Debug.Log($"{label} 다운로드");
        // 1. 카탈로그 업데이트 (기존 코드와 동일)
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;
        // ... (업데이트 로직) ...

        // 2. 해당 라벨을 가진 모든 에셋의 크기 계산
        var sizeHandle = Addressables.GetDownloadSizeAsync(label);
        yield return sizeHandle;

        if (sizeHandle.Result > 0)
        {
            // 3. 해당 라벨의 모든 의존성 다운로드
            var downloadHandle = Addressables.DownloadDependenciesAsync(label);
            while (!downloadHandle.IsDone)
            {
                float progress = downloadHandle.PercentComplete;
                Debug.Log($"전체 패치 진행률: {progress * 100}%");
                yield return null;
            }
            Addressables.Release(downloadHandle);
        }
        Addressables.Release(sizeHandle);
        Debug.Log($"{label} 다운로드 끝");
    }

    public IEnumerator CheckAndDownloadAllAndLoad(string label)
    {
        Debug.Log($"{label} 다운로드");
        // 1. 카탈로그 업데이트 (기존 코드와 동일)
        var checkHandle = Addressables.CheckForCatalogUpdates(false);
        yield return checkHandle;
        // ... (업데이트 로직) ...

        // 2. 해당 라벨을 가진 모든 에셋의 크기 계산
        var sizeHandle = Addressables.GetDownloadSizeAsync(label);
        yield return sizeHandle;

        if (sizeHandle.Result > 0)
        {
            // 3. 해당 라벨의 모든 의존성 다운로드
            var downloadHandle = Addressables.DownloadDependenciesAsync(label);
            while (!downloadHandle.IsDone)
            {
                float progress = downloadHandle.PercentComplete;
                Debug.Log($"전체 패치 진행률: {progress * 100}%");
                yield return null;
            }
            Addressables.Release(downloadHandle);
        }
        Addressables.Release(sizeHandle);

        Debug.Log($"{label} 다운로드 끝");
        yield return LoadAssetsByLabel(label);
    }
    #endregion
}