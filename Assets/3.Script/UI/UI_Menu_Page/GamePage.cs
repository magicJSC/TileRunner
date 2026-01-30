using UnityEngine;

public class GamePage : MonoBehaviour
{
    [SerializeField] UI_EventHandler settingBtn;

    [SerializeField] GameObject settingUIPrefab;
    [SerializeField] AudioClip clickSound;

    Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();

        settingBtn.clickAction = ClickSettingBtn;

        GameManager.Instance.closeSettingAction += BackAnimation;
        GameManager.Instance.startGameAction += StartGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
        GameManager.Instance.closeSettingAction -= BackAnimation;
    }

    void StartGame()
    {
        anim.Play("Start");
    }

    void ClickSettingBtn()
    {
        anim.Play("Setting");
        Instantiate(settingUIPrefab);
        GameManager.Instance.openSettingAction.Invoke();
        SoundManager.Instance.PlayUI(clickSound);
    }

    void BackAnimation()
    {
        anim.Play("Default");
    }
}
