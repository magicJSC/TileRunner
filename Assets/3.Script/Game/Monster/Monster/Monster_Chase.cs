using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;



/// <summary>
/// 몬스터 기본 클래스
/// </summary>
public class Monster_Chase : Monster
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float gravity = -9.81f;

    Transform playerTransform;

    CharacterController controller;

    private Vector3 velocity;
    private void Start()
    {
        playerTransform = GameManager.Instance.player;
        controller = GetComponent<CharacterController>();

        StartCoroutine(ActCor());
    }

    /// <summary>
    /// 행동 반복 코루틴
    /// </summary>
    IEnumerator ActCor()
    {
        while (true)
        {
            Move();

            Rotate();

            yield return null;
        }
    }

    void Move()
    {
        Vector3 dir = playerTransform.position - transform.position;

        dir.y = 0f;

        dir.Normalize();

        // 수평 이동
        Vector3 move = dir * moveSpeed;



        // 중력 적용
        if (controller.isGrounded)
        {
            if (velocity.y < 0)

                velocity.y = -2f; // 바닥에 붙이기

        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 finalMove = move + velocity;

        controller.Move(finalMove * Time.deltaTime);
    }


    void Rotate()
    {
        Vector3 dir = playerTransform.position - transform.position;

        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = targetRot;
    }
}