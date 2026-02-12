using GooglePlayGames;
using UnityEngine;

public class UI_Studio : MonoBehaviour
{
    [SerializeField] UI_Loading loadingUI;

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(result =>
        {
            Debug.Log("Login Result: " + result);
        });
    }

    public void StartLoading()
    {
        gameObject.SetActive(false);
        loadingUI.gameObject.SetActive(true);
    }
}
