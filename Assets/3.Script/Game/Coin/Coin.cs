using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] float disappearTime = 6f;

    public Action disappearAction;

    public void Start()
    {
        StartCoroutine(RotateCor());
        StartCoroutine(DisappearCor());
    }

    private void OnDisable()
    {
        disappearAction?.Invoke();
        disappearAction = null;
    }

    /// <summary>
    /// 사라지는 코루틴
    /// </summary>
    IEnumerator DisappearCor()
    {
        yield return new WaitForSeconds(disappearTime);
        transform.DOScale(Vector3.zero, 0.3f).OnComplete(() => Destroy(gameObject));
    }

    /// <summary>
    /// 회전 코루틴
    /// </summary>
    IEnumerator RotateCor()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            GameManager.Instance.Coin += 1;
            Destroy(gameObject);
        }
    }
}
