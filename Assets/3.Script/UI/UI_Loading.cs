using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class UI_Loading : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] UIFader loadingIcon;

    [Header("Load")]
    StageLoader stageLoader;

    [Header("Scene")]
    [SerializeField] AssetReference sceneRef;

    private void Start()
    { 
        stageLoader = GetComponent<StageLoader>();
        gameObject.SetActive(false);

        AddressableManager.Instance.completeAction += Hide;
    }

    private void OnDestroy()
    {
        AddressableManager.Instance.completeAction -= Hide;
    }

    /// <summary>
    /// 로딩중 UI 보여주고 로딩 시작
    /// </summary>
    public void Show()
    {
        loadingIcon.gameObject.SetActive(true);
        loadingIcon.FadeIn();
        StartLoad();
    } 

    /// <summary>
    /// 로딩 끝, 로딩 UI 숨기기
    /// </summary>
    public void Hide()
    {
        loadingIcon.FadeOut();
        StartCoroutine (HideCor());
    }

    /// <summary>
    /// 로딩 UI 숨긴뒤 씬 넘기기
    /// </summary>
    IEnumerator HideCor()
    {
        yield return new WaitForSeconds(0.2f);
        StartGame();
    }

    public void StartLoad()
    {
        StartCoroutine(stageLoader.StartLoad());
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
