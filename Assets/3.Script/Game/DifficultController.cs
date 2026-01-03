using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시간을 추가하고 시간에 따라서 난이도를 증가시키는 스크립트
/// </summary>
public class DifficultController : MonoBehaviour
{
    public List<DifficultSO> difficultSOList;

    private int levelIndex = 0;
    public int LevelIndex { get { return levelIndex; }
        set
        {
            levelIndex = value;
            levelAction.Invoke(levelIndex);
        }
    }
    public Action<int> levelAction;

    public static DifficultController Instance;

    public float playTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TileManager.Instance.difficultSO = difficultSOList[0];
        BossController.Instance.difficultSO = difficultSOList[0];
        StartCoroutine(TimerCor());
    }

    IEnumerator TimerCor()
    {
        while (true)
        {
            yield return null;
            playTime += Time.deltaTime;
            CheckTimeAndSetSO();
        }
    }

    /// <summary>
    /// 특정 시간이 되면 난이도 레벨 변경
    /// </summary>
    private void CheckTimeAndSetSO()
    {
        // 다음 단계가 없으면 정지
        if (difficultSOList.Count <= LevelIndex + 1)
        {
            return;
        }

        // 시간되면 난이도 상승
        if (difficultSOList[LevelIndex + 1].nextLevelTime <= playTime)
        {
            LevelIndex++;
            TileManager.Instance.difficultSO = difficultSOList[LevelIndex];
            BossController.Instance.difficultSO = difficultSOList[LevelIndex];
        }
    }
}
