using UnityEngine;

[CreateAssetMenu(fileName = "TrialSO", menuName = "Scriptable Objects/TrialSO")]
public class TrialSO : ScriptableObject
{
    public TrialType trialType;
    public Sprite iconImage;
}
