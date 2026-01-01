using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector2Int axialCoord;   // 이 타일의 육각 좌표
    public Material steppedMaterial;

    private Renderer rend;

    protected bool isDisappear;

    private void Start()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();
        Init();
    }

    // 플레이어가 밟았을 때 호출
    public virtual void OnStepped()
    {
        Disappear();
        GameManager.Instance.Score++;
    }

    public virtual void Disappear()
    {
        if (isDisappear)
            return;

        StepAnimation();
        TileManager.Instance.RequestTileCollapse(axialCoord);
        isDisappear = true;
    }

    public virtual void StepAnimation()
    {
        transform.GetChild(0).DOShakePosition(0.65f, 0.5f).onComplete += () => { transform.GetChild(0).DOScale(Vector3.zero, 0.15f); };
        rend.material = steppedMaterial;
    }

    protected virtual void Init()
    {

    }
}
