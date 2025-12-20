using UnityEngine;

public class DropChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver)
            return;
        if(other.TryGetComponent<Player>(out var player))
        {
            GameEnder.Instance.EndGame();
        }
    }
}
