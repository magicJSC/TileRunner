using UnityEngine;

[CreateAssetMenu(fileName = "TimeSO", menuName = "Scriptable Objects/TimeSO")]
public class TimeSO : ScriptableObject
{
    public float collapseDelay = 0.8f;   // 밟고 사라지기까지
    public float respawnDelay = 4.0f;    // 다시 생기기까지
    public float dangerInterval = 6.0f;  // 위험 타일 생성 주기
    public int randomRemoveCount = 1;
    public int nextLevelTime;
}
