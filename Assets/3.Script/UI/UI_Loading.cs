using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Loading : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] UIFader loadingIcon;

    [Header("Load")]
    StageLoader stageLoader;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        stageLoader = GetComponent<StageLoader>();
        gameObject.SetActive(false);

        AddressableManager.Instance.completeAction += Hide;
    }

    private void OnDestroy()
    {
        AddressableManager.Instance.completeAction -= Hide;
    }

    public void Show()
    {
        loadingIcon.gameObject.SetActive(true);
        loadingIcon.FadeIn();
        //StartCoroutine(ShowCor());
        StartLoad();
    } 

    //IEnumerator ShowCor()
    //{
    //    yi
        
    //}

    public void Hide()
    {
        loadingIcon.FadeOut();
        StartCoroutine (HideCor());
    }

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
