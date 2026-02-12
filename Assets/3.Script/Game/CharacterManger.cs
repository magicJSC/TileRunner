using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManger : Singleton<CharacterManger>
{

    [Header("Store")]
    public Camera storeCam;
    public Transform[] characterPoints;

    public Action changeAction;

    [Header("Data")]
    [SerializeField] public List<CharacterData> characterDatas;

    public List<bool> usableList = new List<bool>();

    public int useIndex = 0;

    [Header("Player")]
    [SerializeField]public Transform playerSpawnPos;

    public CharacterData GetCharacterData(int index)
    {
        return characterDatas[index];
    }

    public bool IsCharacterUsable(int index)
    {
        return usableList[index];
    }

    public void ChangeUsable(int index)
    {
        usableList[index] = true;
    }

    public void ChangeCharacter(int index)
    {
        if(GameManager.Instance.Player != null)
            Destroy(GameManager.Instance.Player.gameObject);  

        useIndex = index;
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        Instantiate(characterDatas[useIndex].characterPrefab, playerSpawnPos.position, Quaternion.identity);
    }
}

[Serializable]
public class CharacterData
{
    public int money;
    public GameObject characterPrefab;
}
