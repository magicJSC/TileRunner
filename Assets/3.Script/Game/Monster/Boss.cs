using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("스킬 쿨")]
    [SerializeField] float coolTime;

    [Header("최면")]
    [SerializeField] GameObject hypnosisUI;

    [Header("미사일")]
    [SerializeField] GameObject missle;

    void Start()
    {
        GameManager.Instance.startGameAction += DoSkill;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= DoSkill;
    }

    /// <summary>
    /// 스킬 사용 신호 -> 스킬 선택
    /// </summary>
    void DoSkill()
    {
        StartCoroutine(ChoiceSkill());
    }

    /// <summary>
    /// 스킬 선택
    /// </summary>
    IEnumerator ChoiceSkill()
    {
        yield return new WaitForSeconds(coolTime);
        Missle();
    }

    void Missle()
    {
        Instantiate(missle,TileManager.Instance.GetRandomTile().transform.position, Quaternion.identity);
        DoSkill();
    }
}
