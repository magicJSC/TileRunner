using System.Collections;
using UnityEngine;

public class Monster_Gage : Monster
{
    [SerializeField] float fillCoolTime;
    [SerializeField] float fillAmount;

    [SerializeField] float gravity = -9.81f;

    CharacterController controller;

    Vector3 velocity;

    Coroutine gageCor;

    private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        gageCor = StartCoroutine(FillGageCor());
        StartCoroutine(ActCor());
    }

    IEnumerator FillGageCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(fillCoolTime);
            anim.Play("Skill", -1, 0f);
        }
    }

    public void FillGage()
    {
        BossController.Instance.IncreaseGage(fillAmount);
    }


    /// <summary>
    /// 행동 반복 코루틴
    /// </summary>
    IEnumerator ActCor()
    {
        while (true)
        {
            Move();
            yield return null;
        }
    }

    void Move()
    {
        //중력 적용
        if (controller.isGrounded)
        {
            if (velocity.y < 0)

                velocity.y = -2f; // 바닥에 붙이기

        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public override void Die()
    {
        base.Die();
        StopCoroutine(gageCor);
    }
}
