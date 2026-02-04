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

        // AddressableManager가 AssetReference를 받도록 변경
        yield return AddressableManager.Instance.LoadAssets(preloadAssets);

        loadingUI.Hide();
    }
}
