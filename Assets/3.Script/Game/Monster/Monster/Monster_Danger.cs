using UnityEngine;

public class Monster_Danger : MonoBehaviour
{
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<Player>(out var player))
        {
            player.Die();
        }
    }
}
