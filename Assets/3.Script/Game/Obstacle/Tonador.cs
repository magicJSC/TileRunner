using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Tonador : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    [SerializeField] float minMoveDistance;
    [SerializeField] float existTime;

    Rigidbody rigid;
    Vector3 targetPos;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        MoveRandomTarget();
        StartCoroutine(RoateCor());
        StartCoroutine(DestroyCor());
    }

    private void FixedUpdate()
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        rigid.MovePosition(rigid.position + new Vector3(dir.x, 0, dir.z) * moveSpeed * Time.fixedDeltaTime);
        if ((Mathf.Abs(transform.position.x - targetPos.x) < 0.1f && Mathf.Abs(transform.position.z - targetPos.z) < 0.1f))
        {
            MoveRandomTarget();
        }
    }

    private void MoveRandomTarget()
    {
        while (true)
        {
            targetPos = TileManager.Instance.GetRandomTile().transform.position;
            if ((transform.position - targetPos).magnitude >= minMoveDistance)
            {
                break;
            }
        }
    }

    IEnumerator RoateCor()
    {
        while (true)
        {
            transform.GetChild(0).Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DestroyCor()
    {
        yield return new WaitForSeconds(existTime);
        transform.DOScale(new Vector3(0, transform.localScale.y, 0), 0.5f).onComplete += () => Destroy(gameObject);
    }
}
