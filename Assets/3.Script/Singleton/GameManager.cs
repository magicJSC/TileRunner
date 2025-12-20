using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   public int Score { get { return score; } 
        
        set
        { 
            int getValue = value - score;
            score = value;

            if(isIncreaseDoubleScore)
                score += getValue;

            scoreAction?.Invoke(score);
        } 
    }
    private int score;

    public int bestScore;

    public Action<int> scoreAction;

    public bool isGameOver = false;

    public Action startGameAction;


    // Double Score ฐüทร
    public bool isIncreaseDoubleScore = false;

    public Action doubleScoreAction;
}
