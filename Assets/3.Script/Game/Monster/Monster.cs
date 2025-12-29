using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] float coolTime;

    [Header("스킬 쿨")]
    [SerializeField] GameObject hypnosisUI;
    void Start()
    {
        DoSkill();
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
        Hypnosis();
        //int idx = Random.Range(0, 4);
        //switch (idx)
        //{
        //    case 0:
        //        Hypnosis();
        //        break;
        //    case 1:

        //        break;
        //    case 2:
        //        break;
        //    case 3:
        //        break;
        //}
    }

    void Hypnosis()
    {
        Instantiate(hypnosisUI).GetComponent<IEndSignal>().AddEndSignalListener(DoSkill);
    }
}
