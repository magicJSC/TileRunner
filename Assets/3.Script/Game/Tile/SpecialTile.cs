using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialTile : HexTile
{
    [SerializeField] float coolTime;
    protected float curTime;

    private Image coolImage; 

    protected override void Init()
    {
        curTime = coolTime;
        coolImage  = Util.FindChild<Image>(gameObject, "CurTime");
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while(curTime > 0)
        {
            if(isStepped)
                yield break;
            curTime -= Time.deltaTime;
            coolImage.fillAmount = curTime / coolTime;
            yield return null;
        }

        TimerOver();
    }

    /// <summary>
    /// 쿨타임이 끝났을 때 실행
    /// </summary>
    public virtual void TimerOver()
    {
        TileManager.Instance.RequestTileCollapse(axialCoord);
        transform.DOScale(Vector3.zero, 0.4f);
    }
}
