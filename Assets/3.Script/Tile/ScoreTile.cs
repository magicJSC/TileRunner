using DG.Tweening;
using UnityEngine;

public class ScoreTile : SpecialTile
{
    public override void OnStepped()
    {
        base.OnStepped();
        
    }

    public override void TimerOver()
    {
        TileManager.Instance.RequestTileCollapse(axialCoord);
        transform.DOScale(Vector3.zero, 0.8f);
    }
}
