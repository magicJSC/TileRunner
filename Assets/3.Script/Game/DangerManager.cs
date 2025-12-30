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
        var timeController = GetComponent<DifficultController>();
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
            TileManager.Instance.DissapearTile(coord);
        }

        // 안개 생성
        if (curDangerSO.fog)
        {
            Instantiate(fog,tileTransform.transform.position,Quaternion.identity);
        }

        // 토네이도 스폰
        if(curDangerSO.tornadoCount != 0)
        {
            StartCoroutine(SpawnTornador(curDangerSO.tornadoCount, tileTransform.transform.position + new Vector3(0, 0.8f,0)));
        }
    }
    
    IEnumerator SpawnTornador(int count, Vector3 tilePos)
    {
        for(int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(tornadorSpawnTerm);
            Instantiate(tornador, tilePos, Quaternion.identity);
        }
    }
}
