using DG.Tweening;
using UnityEngine;

/// <summary>
/// 나갈 때만 사라지는 시작 타일
/// </summary>
public class StartTile : MonoBehaviour
{
    public Vector2Int axialCoord;   // 이 타일의 육각 좌표

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            TileManager.Instance.RequestTileCollapse(axialCoord);
            StepAniation();
        }
    }

    public void StepAniation()
    {
        transform.GetChild(0).DOShakePosition(0.65f, 0.5f).onComplete += () => { transform.GetChild(0).DOScale(Vector3.zero, 0.15f); };
    }
}
