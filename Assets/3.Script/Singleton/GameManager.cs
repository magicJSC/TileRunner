using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
   public int Score { get { return score; }  set { score = value; scoreAction?.Invoke(score); } }
    private int score;

    public Action<int> scoreAction;
}
