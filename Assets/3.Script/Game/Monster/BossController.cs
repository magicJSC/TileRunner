using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController Instance;

    public float bossMaxGage = 100f;
    public float BossGage 
    { 
        get
        {
            return bossGage; 
        }
        set
        {
            if (bossGage >= bossMaxGage)
                return;

            bossGage = value; 
            bossGageAction?.Invoke(bossGage / bossMaxGage);
        }
    }
    private float bossGage;

    public DifficultSO difficultSO;

    public Action<float> bossGageAction;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BossGage = 0;
        GameManager.Instance.startGameAction += StartBossGageCor;
    }

    private void OnDisable()
    {
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
            yield return null;
            BossGage += Time.deltaTime * difficultSO.bossGageAmount;
        }
    }

    //void Missle()
    //{
    //    Instantiate(missle,TileManager.Instance.GetRandomTile().transform.position, Quaternion.identity);
    //}
}
