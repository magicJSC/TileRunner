using DG.Tweening;
using UnityEngine;

public class Monster_missle : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f).onComplete += Move;
        Destroy(gameObject,5);
    }

    void Move()
    {
        Vector3 dir = (GameManager.Instance.player.position - transform.position).normalized;
        Vector3 moveDir = new Vector3(dir.x, 0, dir.z).normalized;
        rigid.linearVelocity = moveDir * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HexTile>(out var hexTile))
        {
            hexTile.Disappear();
        }
    }
}
