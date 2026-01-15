using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // 외부에서 접근하기 위한 읽기 전용 인스턴스
    public static BossController Instance { get; private set; }

    [Header("Gage Settings")]
    public float bossMaxGage = 100f;
    private float bossGage;

    [Header("Skill Settings")]
    [SerializeField] float skillCoolTime = 5f;
    [SerializeField] List<BossSO> bossSOList;

    private int curSOIndex = 0;

    // 프로퍼티 수정: 내부 로직에서 Instance를 참조하지 않도록 변경
    public float BossGage
    {
        get => bossGage;
        set
        {
            bossGage = Mathf.Clamp(value, 0, bossMaxGage);

            SetSkillSO();

            // UI 업데이트 등을 위한 액션 실행
            bossGageAction?.Invoke(bossGage / bossMaxGage);
        }
    }

    [HideInInspector]
    public DifficultSO difficultSO;
    public Action<float> bossGageAction;


    [Header("Skill")]
    [SerializeField] GameObject skillPrefab;
    [SerializeField] Transform skillPos;
    Animator anim;

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 내부 변수에 직접 접근하거나 프로퍼티 사용
        BossGage = 0;
        anim = GetComponent<Animator>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.startGameAction += StartAct;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.startGameAction -= StartAct;
        }
    }

    void StartAct()
    {
        StartCoroutine(BossGageCor());
        StartCoroutine(SkillCor());
    }

    IEnumerator BossGageCor()
    {
        while (true)
        {
            // 매 프레임 게이지 상승
            BossGage += Time.deltaTime * difficultSO.bossGageAmount;
            yield return null;
        }
    }


    public void DecreaseGage(float amount)
    {
        BossGage -= amount;
    }

    public void IncreaseGage(float amount)
    {
        BossGage += amount;
    }

    void SetSkillSO()
    {
      for(int i = 0; i < bossSOList.Count; i++)
        {
            if (BossGage >= bossSOList[i].bossGage)
            {
                curSOIndex = i;
            }
            else
                break;
        }
    }

    IEnumerator SkillCor()
    {
        while (true)
        {
            anim.Play("Skill", -1, 0);
            yield return new WaitForSeconds(skillCoolTime);
        }
    }

    public void Skill()
    {
        GameObject skillGO = Instantiate(skillPrefab, skillPos.position, Quaternion.identity);
        skillGO.GetComponent<Boss_Skill>().skillScale = bossSOList[curSOIndex].skillScale;
    }
}