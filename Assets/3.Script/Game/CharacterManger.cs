using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManger : MonoBehaviour
{
    public static CharacterManger Instance { get; private set; }

    [Header("Store")]
    public Camera storeCam;
    public Transform[] characterPoints;

    public Action changeAction;

    [Header("Data")]
    [SerializeField] public List<CharacterData> characterDatas;

    [HideInInspector]
    public List<bool> usableList;

    public int useIndex = 0;

    [Header("Player")]
    [SerializeField]public Transform playerSpawnPos;

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
            Instance = this;
        InitUsableList();
    }
    
    void InitUsableList()
    {
        usableList = new List<bool>();
        for (int i = 0; i < characterDatas.Count; i++)
        {
            usableList.Add(false);
        }

        // 처음 캐릭터 2개는 기본으로 사용 가능
        usableList[0] = true;
        usableList[1] = true;

        ChangeCharacter(useIndex);
    }

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
        Instantiate(characterDatas[index].characterPrefab, playerSpawnPos.position, Quaternion.identity);
    }
}

[Serializable]
public class CharacterData
{
    public int money;
    public GameObject characterPrefab;
}
