using UnityEngine;

/// <summary>
/// 임시 세팅, 치트 사용 툴
/// </summary>
public class MapSettingTool : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] HexMapSO hexMapSO;
    [SerializeField] int rotationStep = 0;

    /// <summary>
    /// 버튼 클릭 시 맵 생성
    /// </summary>
    public void OnClickedGenerateMapButton()
    {
        TileManager.Instance.LoadMap(hexMapSO ,rotationStep);
    } 

    /// <summary>
    /// 버튼 클릭 시 맵 삭제
    /// </summary>
    public void OnClickDeleteMapButton()
    {
        TileManager.Instance.DeleteMap();
    }
}
