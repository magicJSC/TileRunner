using UnityEngine;

public class StartTile : MonoBehaviour
{
    public Vector2Int axialCoord;   // ¿Ã ≈∏¿œ¿« ¿∞∞¢ ¡¬«•

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out Player player))
        {
            TileManager.Instance.RequestTileCollapse(axialCoord);
        }
    }
}
