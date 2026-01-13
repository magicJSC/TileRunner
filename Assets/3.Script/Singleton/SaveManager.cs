using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    string bestScoreKey = "save_bestScore";
    string coinKey = "save_coin";

    protected override void Init()
    {
        LoadData();
    }

    public void InitData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat(bestScoreKey, 0);
        PlayerPrefs.SetFloat(coinKey, 0);
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat(bestScoreKey, GameManager.Instance.bestScore);
        PlayerPrefs.SetFloat(coinKey, GameManager.Instance.Coin);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(bestScoreKey))
        {
            GameManager.Instance.Coin = (int)PlayerPrefs.GetFloat(coinKey);
            GameManager.Instance.Score = (int)PlayerPrefs.GetFloat(bestScoreKey);
        }
        else
            InitData();
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}