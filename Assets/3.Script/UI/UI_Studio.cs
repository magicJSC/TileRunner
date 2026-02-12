
using UnityEngine;

public class UI_Studio : MonoBehaviour
{
    [SerializeField] UI_Loading loadingUI;

    public void StartLoading()
    {
        gameObject.SetActive(false);
        loadingUI.gameObject.SetActive(true);
    }
}
