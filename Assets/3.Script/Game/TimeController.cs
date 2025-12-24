using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시간을 추가하고 시간에 따라서 난이도를 증가시키는 스크립트
/// </summary>
public class TimeController : MonoBehaviour
{
    public List<TimeSO> timeSOList;

    private int time = 0;

    private int levelIndex = 0;
    public int LevelIndex { get { return levelIndex; }
        set
        {
            levelIndex = value;
            levelAction.Invoke(levelIndex);
        }
    }
    public Action<int> levelAction;

    private Coroutine timerCoroutine;

    private void Start()
    {
        GameManager.Instance.startGameAction += StartGame;
        TileManager.Instance.timeSO = timeSOList[0];
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
    }

    private void StartGame()
    {
        timerCoroutine = StartCoroutine(Timer());
    }

   IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            time++;
            CheckTimeSO();
        }
    }

    /// <summary>
    /// 특정 시간이 되면 난이도 레벨 변경
    /// </summary>
    private void CheckTimeSO()
    {
        // 다음 단계가 없으면 정지
        if (timeSOList[LevelIndex + 1] == null)
        {
            StopCoroutine(timerCoroutine);
            return;
        }

        // 시간되면 난이도 상승
        if (timeSOList[LevelIndex + 1].nextLevelTime <= time)
        {
            LevelIndex++;
            TileManager.Instance.timeSO = timeSOList[LevelIndex];
        }
    }
}
