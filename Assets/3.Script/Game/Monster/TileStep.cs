using UnityEngine;

public class TileStep : MonoBehaviour
{
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent<HexTile>(out var tile))
        {
            tile.Disappear();
        }
    }
}
