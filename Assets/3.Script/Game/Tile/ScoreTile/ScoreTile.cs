using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTile : SpecialTile
{
    public override void OnStepped()
    {
        if(curTime <= 0 || isStepped) return;

        base.OnStepped();
        GameManager.Instance.doubleScoreAction?.Invoke();
    }
}
