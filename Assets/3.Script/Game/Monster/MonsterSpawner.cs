using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [SerializeField] List<MonsterSpawnSO> monsterSpawnSOList;

    private MonsterSpawnSO curMonsterSpawnSO;

    private List<int> availableIndexList = new List<int>();
    Transform playerTransform;
    private void Start()
    {
        DifficultController.Instance.levelAction += GetSpawnSO;
        playerTransform = GameManager.Instance.player;
        GetSpawnSO(0);

        GameManager.Instance.startGameAction += StartSpawn;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartSpawn;
    }

    void GetSpawnSO(int level)
    {
        if(monsterSpawnSOList.Count <= level)
        {
            DifficultController.Instance.levelAction -= GetSpawnSO;
            return;
        }
        curMonsterSpawnSO = monsterSpawnSOList[level];

        // 인덱스 풀 초기화
        availableIndexList.Clear();
        for (int i = 0; i < curMonsterSpawnSO.spawnList.Count; i++)
        {
            availableIndexList.Add(i);
        }
    }

    void StartSpawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(curMonsterSpawnSO.spawnInterval);

            if (availableIndexList.Count == 0)
                continue;

            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        // 랜덤 인덱스 선택
        int listIdx = Random.Range(0, availableIndexList.Count);
        int spawnIndex = availableIndexList[listIdx];

        // 인덱스 제거 (중복 방지)
        availableIndexList.RemoveAt(listIdx);

        GameObject prefab = curMonsterSpawnSO.spawnList[spawnIndex];

        GameObject monsterGO = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);

        Monster monster = monsterGO.GetComponent<Monster>();
        monster.Init(spawnIndex, DifficultController.Instance.LevelIndex, ReturnSpawnIndex);
    }

    Vector3 GetSpawnPosition()
    {
        return TileManager.Instance.GetRandomTilePosition(playerTransform, minDistance, maxDistance);
    }

    // 몬스터 사망 시 호출
    public void ReturnSpawnIndex(int index, int level)
    {
        if (DifficultController.Instance.LevelIndex != level)
            return;

        if (!availableIndexList.Contains(index))
            availableIndexList.Add(index);
    }
}
