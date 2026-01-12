using System;
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

    public Action startGameAction;
    public Action endGameAction;

    public Transform player;
}
