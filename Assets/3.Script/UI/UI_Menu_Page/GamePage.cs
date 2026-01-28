using UnityEngine;

public class GamePage : MonoBehaviour
{
    [SerializeField] UI_EventHandler settingBtn;

    [SerializeField] GameObject settingUIPrefab;

    Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();

        settingBtn.clickAction = ClickSettingBtn;

        GameManager.Instance.startGameAction += StartGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
    }

    void StartGame()
    {
        anim.Play("Start");
    }

    void ClickSettingBtn()
    {
        anim.Play("Setting");
        Instantiate(settingUIPrefab).GetComponent<UI_Setting>().backAction += BackAnimation;
    }

    void BackAnimation()
    {
        anim.Play("Default");
    }
}
