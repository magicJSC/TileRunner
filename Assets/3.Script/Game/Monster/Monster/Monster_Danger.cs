using UnityEngine;

public class Monster_Danger : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            player.Die();
        }
    }
}
