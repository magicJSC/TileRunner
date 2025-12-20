using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTile : SpecialTile
{
    bool isTimeOver;

    TrialType trialType;
    Image iconImage;

    public void Init(TrialSO trialSO)
    {
        iconImage = Util.FindChild<Image>(gameObject,"Icon");

        trialType = trialSO.trialType;
        iconImage.sprite = trialSO.iconImage;
    }

    public override void OnStepped()
    {
        if(isTimeOver) return;

        base.OnStepped();
        GameManager.Instance.doubleScoreAction?.Invoke();
        TrialExecuter.Instance.ExcuteTrial(trialType);
    }

    public override void TimerOver()
    {
        TileManager.Instance.RequestTileCollapse(axialCoord);
        transform.DOScale(Vector3.zero, 0.8f);
        isTimeOver = true;
    }
}
