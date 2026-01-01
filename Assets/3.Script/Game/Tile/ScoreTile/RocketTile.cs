using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RocketTile : SpecialTile
{
    public override void OnStepped()
    {
        if(curTime <= 0 || isDisappear) return;

        base.OnStepped();
        GameManager.Instance.Score++;
    }
}
