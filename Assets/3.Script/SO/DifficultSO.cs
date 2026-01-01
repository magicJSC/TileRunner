using UnityEngine;

[CreateAssetMenu(fileName = "DifficultSO", menuName = "Scriptable Objects/DifficultSO")]
public class DifficultSO : ScriptableObject
{
    public float collapseDelay = 0.8f;   // 밟고 사라지기까지
    public float respawnDelay = 4.0f;    // 다시 생기기까지
    public float bossGageAmount = 6.0f;  // 1초에 x%
    public int dangerCount = 1;
    public int nextLevelTime;
}
