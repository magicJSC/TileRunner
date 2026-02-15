using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    string bestScoreKey = "save_bestScore";
    string coinKey = "save_coin";
    string skinKey = "save_skin";
    string useKey = "save_use";

    protected override void Init()
    {
        LoadData();
    }

    public void InitData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat(bestScoreKey, 0);
        PlayerPrefs.SetFloat(coinKey, 0);
        PlayerPrefs.SetInt(useKey, 0);

        for (int i = 0; i < CharacterManger.Instance.characterDatas.Count; i++)
        {
            PlayerPrefs.SetInt(skinKey + i, i < 2 ? 1 : 0);
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat(bestScoreKey, GameManager.Instance.bestScore);
        PlayerPrefs.SetFloat(coinKey, GameManager.Instance.Coin);
        PlayerPrefs.SetInt(useKey, CharacterManger.Instance.useIndex);

        for (int i = 0; i < CharacterManger.Instance.usableList.Count; i++)
        {
            PlayerPrefs.SetInt(skinKey+i,CharacterManger.Instance.usableList[i] ? 1 : 0);
        }
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(bestScoreKey))
        {
            GameManager.Instance.Coin = (int)PlayerPrefs.GetFloat(coinKey);
            GameManager.Instance.bestScore = (int)PlayerPrefs.GetFloat(bestScoreKey);
            CharacterManger.Instance.useIndex = PlayerPrefs.GetInt(useKey);

            for (int i = 0; i < CharacterManger.Instance.characterDatas.Count; i++)
            {
                CharacterManger.Instance.usableList.Add(PlayerPrefs.GetInt(skinKey + i) == 1);
            }
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