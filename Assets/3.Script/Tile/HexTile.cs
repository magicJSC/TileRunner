using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector2Int axialCoord;   // 이 타일의 육각 좌표
    public int score;

    private bool isStepped;

    // 플레이어가 밟았을 때 호출
    public virtual void OnStepped()
    {
        if (isStepped)
            return;

        TileManager.Instance.RequestTileCollapse(axialCoord);
        GameManager.Instance.Score += score;
        isStepped = true;
        StepAniation();
    }

    public void StepAniation()
    {
        transform.GetChild(0).DOShakePosition(0.65f, 0.5f).onComplete += () => { transform.GetChild(0).DOScale(Vector3.zero, 0.15f); };
    }
}
