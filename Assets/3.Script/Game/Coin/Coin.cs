using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 100f;

    public Action disappearAction;

    public void Start()
    {
        StartCoroutine(RotateCor());
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
}
