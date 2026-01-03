using UnityEngine;

public class DropChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver)
            return;
        if(other.TryGetComponent<Player>(out var player))
        {
            player.Fall();
        }
        if (other.TryGetComponent<Monster_Chase>(out var monster))
        {
            monster.Die();
        }
    }
}
