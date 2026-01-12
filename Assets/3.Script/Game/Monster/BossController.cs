using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // 외부에서 접근하기 위한 읽기 전용 인스턴스
    public static BossController Instance { get; private set; }

    [Header("Gage Settings")]
    public float bossMaxGage = 100f;
    private float bossGage;


    bool isFilled;

    // 프로퍼티 수정: 내부 로직에서 Instance를 참조하지 않도록 변경
    public float BossGage
    {
        get => bossGage;
        set
        {
            // 최대치 도달 시 더 이상 증가하지 않음
            if (isFilled)
                return;

            bossGage = Mathf.Clamp(value, 0, bossMaxGage);

            if (bossGage >= bossMaxGage)
            {
                isFilled = true;
                StartCoroutine(SkillCor());
            }


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
            GameManager.Instance.startGameAction += StartBossGageCor;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.startGameAction -= StartBossGageCor;
    }

    void StartBossGageCor()
    {
        StartCoroutine(BossGageCor());
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

    IEnumerator SkillCor()
    {
        while (true)
        {
            anim.Play("Skill", -1, 0);
            yield return new WaitForSeconds(3);
        }
    }

    public void Skill()
    {
        Instantiate(skillPrefab, skillPos.position, Quaternion.identity);
    }
}