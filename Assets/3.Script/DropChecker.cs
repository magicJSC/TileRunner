using UnityEngine;

public class DropChecker : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameOver)
            return;
        if(other.TryGetComponent<Player>(out var player))
        {
            GameManager.Instance.isGameOver = true;
            Instantiate(gameOverUI);
        }
    }
}
