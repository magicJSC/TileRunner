using DG.Tweening;
using UnityEngine;

public class Boss_Skill : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody rigid;

    bool isHited;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 1f).onComplete += Move;

        Destroy(gameObject, 10);
    }

    void Move()
    {
        Vector3 dir = (GameManager.Instance.player.position - transform.position).normalized;
        rigid.linearVelocity = dir * moveSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HexTile>(out var hexTile))
        {
            if (!isHited)
            {
                isHited = true;
                transform.DOScale(new Vector3(10, 10, 10), 4f).onComplete +=
                    () => transform.DOScale(Vector3.zero,1).onComplete += 
                    () => Destroy(gameObject);
                rigid.linearVelocity = Vector3.zero;
            }

            hexTile.Disappear();
        }
    }
}
