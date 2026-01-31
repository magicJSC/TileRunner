using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Start : MonoBehaviour
{
    [SerializeField] float playTerm;
    [SerializeField] AudioClip music;
    Canvas canvas;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();
        GameManager.Instance.startGameAction += StartGame;
        GameManager.Instance.openSettingAction += PlayOpenSetting;
        GameManager.Instance.beforeChangeSettingAction += PlayChangeSetting;
        GameManager.Instance.closeSettingAction += PlayCloseSetting;

        StartCoroutine(PlayMusic());
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
        GameManager.Instance.openSettingAction -= PlayOpenSetting;
        GameManager.Instance.beforeChangeSettingAction -= PlayChangeSetting;
        GameManager.Instance.closeSettingAction -= PlayCloseSetting;
    }

    IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(playTerm);
        SoundManager.Instance.PlayBGM(music, true);
    }

    void StartGame()
    {
        Destroy(gameObject);
        anim.Play("Start");
    }

    void PlayOpenSetting()
    {
        anim.Play("Open");
    }

    void PlayCloseSetting()
    {
        anim.Play("Close");
    }

    void PlayChangeSetting()
    {
        anim.Play("Change");

    }

    public void ChangeSetting()
    {
        GameManager.Instance.changeSettingAction.Invoke();
    }

    public void SetSortOrder(int sortOrder)
    {
        canvas.sortingOrder = sortOrder;
    }
}
