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
            if(isDisappear)
                yield break;
            curTime -= Time.deltaTime;
            coolImage.fillAmount = curTime / coolTime;
            yield return null;
        }

        TimerOver();
    }

    /// <summary>
    /// ƒ≈∏¿”¿Ã ≥°≥µ¿ª ∂ß Ω««‡
    /// </summary>
    public virtual void TimerOver()
    {
        Disappear();
    }

    public override void Disappear()
    {
        if (isDisappear)
            return;

        base.Disappear();
        GetComponentInChildren<Light>().enabled = false;
    }
}
