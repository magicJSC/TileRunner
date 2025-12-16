using System.Collections;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public Vector2Int axialCoord;   // 이 타일의 육각 좌표

    // 플레이어가 밟았을 때 호출
    public virtual void OnStepped()
    {
        TileManager.Instance.RequestTileCollapse(axialCoord);
    }

}
