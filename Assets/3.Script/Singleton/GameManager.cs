using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   public int Score { get { return score; }  set { score = value; scoreAction?.Invoke(score); } }
    private int score;

    public int bestScore;

    public Action<int> scoreAction;

    public bool isGameOver = false;

    public Action startGameAction;
}
