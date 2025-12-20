using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpecialTile : HexTile
{
    [SerializeField] float coolTime;
    private float curTime;

    private Image coolImage; 

    private void Start()
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

    public virtual void TimerOver()
    {
        TileManager.Instance.RequestTileCollapse(axialCoord);
        StepAniation();
    }
}
