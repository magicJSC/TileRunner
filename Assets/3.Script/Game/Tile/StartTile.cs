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

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * 0.35f, 0.5f);

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
        var body = transform.GetChild(0);

        body.DOKill();

        body
            .DOShakePosition(0.65f, 0.5f)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                if (this == null) return;

                body
                    .DOScale(Vector3.zero, 0.15f)
                    .SetLink(gameObject)
                    .OnComplete(() =>
                        TileManager.Instance.RemoveTile(axialCoord)
                    );
            });
        rend.material = steppedMaterial;
    }

    void StartGame()
    {
        isStart = true;
    }

    protected virtual void OnDestroy()
    {
        transform.DOKill();
    }
}
