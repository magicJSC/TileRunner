using UnityEngine;

/// <summary>
/// 치트 툴
/// </summary>
public class CheatTool : MonoBehaviour
{
    [Header("돈 추가 치트")]
    [SerializeField] int addMoneyAmount;

    /// <summary>
    /// 버튼 클릭 시 돈 추가
    /// </summary>
    public void OnClickedAddMoneyButton()
    {
        GameManager.Instance.Coin += addMoneyAmount;
    }
}
