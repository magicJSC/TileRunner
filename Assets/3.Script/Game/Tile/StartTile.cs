using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 나갈 때만 사라지는 시작 타일
/// </summary>
public class StartTile : MonoBehaviour, ITile
{
    public Vector2Int axialCoord;   // 이 타일의 육각 좌표
    public Material steppedMaterial;

    private Renderer rend;

    private bool isStart;

    private void Start()
    {
        rend = transform.GetChild(0).GetComponent<Renderer>();

        GameManager.Instance.startGameAction += StartGame;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartGame;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isStart)
            return;

        if(other.TryGetComponent(out Player player))
        {
            StepAniation();
        }
    }

    public void StepAniation()
    {
        transform.GetChild(0).DOShakePosition(0.65f, 0.5f).onComplete += () =>
        {
            transform.GetChild(0).DOScale(Vector3.zero, 0.15f).onComplete += () => TileManager.Instance.RemoveTile(axialCoord);
        };
        rend.material = steppedMaterial;
    }

    void StartGame()
    {
        isStart = true;
    }
}
