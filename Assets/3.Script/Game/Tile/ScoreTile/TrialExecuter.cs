using UnityEngine;

public class TrialExecuter : MonoBehaviour
{
    [Header("메테오")]
    [SerializeField] GameObject meteorSpawner;


    [Header("레이저")]
    [SerializeField] GameObject laser;

    public static TrialExecuter Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// TrialType에 따라서 시련 실행
    /// </summary>
    /// <param name="trialType"></param>
    public void ExcuteTrial(TrialType trialType)
    {
        switch (trialType)
        {
            case TrialType.Meteor:
                Meteor();
                break;
            case TrialType.Laser: 
                Laser();
                break;
        }
    }

    /// <summary>
    /// 메테오 생성
    /// </summary>
    private void Meteor()
    {
        Instantiate(meteorSpawner);
    }

    /// <summary>
    /// 레이저 생성
    /// </summary>
    private void Laser()
    {
        Instantiate(laser);
    }
}
