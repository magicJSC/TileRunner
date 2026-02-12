using System.Collections;
using UnityEngine;

public class UI_MoveHelp : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.startGameAction += StartAction;
        GameManager.Instance.restartAction += RestartAction;
    }

    private void OnDestroy()
    {
        GameManager.Instance.startGameAction -= StartAction;
        GameManager.Instance.restartAction -= RestartAction;
    }

    void StartAction()
    {
        gameObject.SetActive(false);
    }

    void RestartAction()
    {
        gameObject.SetActive(true);
    }

}
