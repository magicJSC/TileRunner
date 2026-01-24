using UnityEngine;

public class UI_Start : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.startGameAction += StartGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
    }

    void StartGame()
    {
        Destroy(gameObject);
    }
}
