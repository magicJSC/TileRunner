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

    // 프로퍼티 수정: 내부 로직에서 Instance를 참조하지 않도록 변경
    public float BossGage
    {
        get => bossGage;
        set
        {
            // 최대치 도달 시 더 이상 증가하지 않음
            if (bossGage >= bossMaxGage && value > bossGage)
                return;

            bossGage = Mathf.Clamp(value, 0, bossMaxGage);

            // UI 업데이트 등을 위한 액션 실행
            bossGageAction?.Invoke(bossGage / bossMaxGage);
        }
    }

    public DifficultSO difficultSO;
    public Action<float> bossGageAction;

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
            // 매 프레임 게이지 상승 (내부 프로퍼티 사용)
            BossGage += Time.deltaTime * difficultSO.bossGageAmount;
            yield return null;
        }
    }

    // 외부(예: Monster 스크립트)에서 게이지를 줄일 때 호출할 함수
    public void DecreaseGage(float amount)
    {
        BossGage -= amount;
    }
}