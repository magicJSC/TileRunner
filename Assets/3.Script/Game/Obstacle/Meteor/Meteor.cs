using System.Collections;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] GameObject targetSignPrefab;
    GameObject targetSign;

    [SerializeField] float speed;

    LineRenderer lineRenderer;

    private void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();


        targetSign = Instantiate(targetSignPrefab, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.Euler(90,0,0));

        rigid.linearVelocity = Vector3.down * speed;
        StartCoroutine(SetTargetSignCor());
    }

    /// <summary>
    /// 떨어지는 목표 표시를 세팅해주는 코루틴
    /// </summary>
    IEnumerator SetTargetSignCor()
    {
        while (true)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, new Vector3(transform.position.x, 0.2f, transform.position.z));

            if(transform.position.y < targetSign.transform.position.y)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            //player.Die();
            Destroy(gameObject);
        }
        else if (other.TryGetComponent(out HexTile tile))
        {
            tile.StepAnimation();
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        Destroy(targetSign);
    }
}
