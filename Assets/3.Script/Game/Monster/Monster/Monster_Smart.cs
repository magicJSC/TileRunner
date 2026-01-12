using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Monster_Smart : Monster
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float waitTime = 1.5f;

    [Header("Tile Detection")]
    [SerializeField] Vector3 boxHalfExtents = new Vector3(0.4f, 0.2f, 0.4f); // 체크할 박스의 크기 (가로, 높이, 세로)
    [SerializeField] float checkDistance = 1.0f; // 몬스터 중심에서 얼마나 앞을 볼 것인가
    [SerializeField] LayerMask tileLayer;

    CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentDir;
    private bool isThinking = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentDir = GetRandomDirection();
        StartCoroutine(ActCor());
    }

    IEnumerator ActCor()
    {
        while (true)
        {
            if (!isThinking)
            {
                // 박스 체크로 변경
                if (!IsTileAhead())
                {
                    yield return StartCoroutine(ThinkRoutine());
                }
                else
                {
                    Move();
                    RotateTowardsMove();
                }
            }
            Gravity();
            yield return null;
        }
    }

    bool IsTileAhead()
    {
        // 몬스터의 현재 방향을 기준으로 앞쪽 아래 지점 계산
        Vector3 checkCenter = transform.position + (currentDir * checkDistance) + (Vector3.down * 0.8f);

        // Physics.CheckBox는 해당 영역 안에 콜라이더가 하나라도 있으면 true를 반환합니다.
        // 타일 간의 간격이 있더라도 박스 크기(boxHalfExtents)가 그 간격보다 크면 무시하고 지나갑니다.
        return Physics.CheckBox(checkCenter, boxHalfExtents, Quaternion.identity, tileLayer);
    }

    IEnumerator ThinkRoutine()
    {
        isThinking = true;

        // 멈췄을 때 즉시 속도 초기화 (미끄러짐 방지)
        velocity = Vector3.zero;

        yield return new WaitForSeconds(waitTime);

        // 새로운 랜덤 방향 설정
        currentDir = GetRandomDirection();

        // 방향을 틀자마자 즉시 회전시켜서 다음 IsTileAhead가 새로운 방향을 보게 함
        if (currentDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(currentDir);

        isThinking = false;
    }

    Vector3 GetRandomDirection()
    {
        float angle = UnityEngine.Random.Range(0f, 360f);
        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

    void Move()
    {
        Vector3 move = currentDir * moveSpeed;

        controller.Move((move + velocity) * Time.deltaTime);
    }

    void Gravity()
    {
        if (controller.isGrounded)
        {
            if (velocity.y < 0) velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    void RotateTowardsMove()
    {
        //if (currentDir.sqrMagnitude < 0.001f) return;
        //Quaternion targetRot = Quaternion.LookRotation(currentDir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
    }


    // 에디터 뷰에서 박스 범위를 시각적으로 확인 (매우 중요!)
    private void OnDrawGizmos()
    {
        Gizmos.color = IsTileAhead() ? Color.green : Color.red;

        // 현재 방향을 기준으로 박스 위치 계산
        Vector3 checkCenter = transform.position + (currentDir * checkDistance) + (Vector3.down * 0.5f);

        // 박스 그리기
        Gizmos.DrawWireCube(checkCenter, boxHalfExtents * 2);
    }

    public override void Die()
    {
        base.Die();
        BossController.Instance.DecreaseGage(declineAmount);
    }
}