using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerManager : MonoBehaviour
{
    [Header("DangerSO")]
    [SerializeField] List<DangerSO> dangerList;

    [Header("오브젝트")]
    [SerializeField] GameObject fog;
    [SerializeField] GameObject tornador;
    [SerializeField] float tornadorSpawnTerm;

    private DangerSO curDangerSO;

    public static DangerManager Instance;

    private void Start()
    {
        var timeController = GetComponent<TimeController>();
        timeController.levelAction += GetDangerSO;
        GetDangerSO(timeController.LevelIndex);

        Instance = this;
    }

    void GetDangerSO(int level)
    {
        var changeSO = dangerList.Find(x => x.levelIndex == level);
        if (changeSO != null)
        {
            curDangerSO = changeSO;
        }
    }

    /// <summary>
    /// 위험 타일 밟을시 실행 함수
    /// </summary>
    /// <param name="tileTransform"></param>
    public void Execute(Transform tileTransform)
    {
        // 타일 삭제
        var coordList = TileManager.Instance.GetRandomActivateCoord(curDangerSO.removeCount);
        foreach (var coord in coordList)
        {
            TileManager.Instance.SteppedTile(coord);
        }

        // 안개 생성
        if (curDangerSO.fog)
        {
            Instantiate(fog);
        }

        // 토네이도 스폰
        if(curDangerSO.tornadoCount != 0)
        {
            StartCoroutine(SpawnTornador(curDangerSO.tornadoCount, tileTransform));
        }
    }
    
    IEnumerator SpawnTornador(int count, Transform tileTransform)
    {
        for(int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(tornadorSpawnTerm);
            Instantiate(tornador, tileTransform.position,Quaternion.identity);
        }
    }
}
