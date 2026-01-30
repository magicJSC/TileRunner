using System;
using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region 점수
    public int Score { get { return score; } 
        
        set
        { 
            score = value;
            scoreAction?.Invoke(score);
        } 
    }

    private int score;

    public int bestScore;

    public Action<int> scoreAction;
    #endregion

    #region 코인
    public int Coin
    {
        get { return coin; }

        set
        {
            coin = value;

            coinAction?.Invoke(coin);
        }
    }
    private int coin;
    

    public Action<int> coinAction;
    #endregion


    public bool isGameOver = false;
    public bool isStart;

    public Action openSettingAction;
    public Action changeSettingAction;
    public Action beforeChangeSettingAction;
    public Action closeSettingAction;

    public Action startGameAction;
    public Action endGameAction;
    public Action resetAction;
    public Action restartAction;

    private Transform player;
    public Transform Player { get { return player; } set { player = value; playerAction?.Invoke(); } }

    public Action playerAction;
}
