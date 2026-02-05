using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    UI_Loading loadingUI;

    [Header("Preload Addressable Assets")]
    [SerializeField] private List<AssetReference> preloadAssets;

    private void Awake()
    {
        loadingUI = GetComponent<UI_Loading>();
    }

    public IEnumerator StartLoad()
    {

        if (loadingUI == null)
            loadingUI = GetComponent<UI_Loading>();

        yield return new WaitForSeconds(0.2f);

        // AddressableManager가 AssetReference를 받도록 변경
        yield return AddressableManager.Instance.CheckAndDownload(preloadAssets);

        loadingUI.Hide();
    }
}
