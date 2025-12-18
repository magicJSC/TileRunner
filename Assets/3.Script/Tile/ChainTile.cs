using UnityEngine;

public class ChainTile : HexTile
{
    public override void OnStepped()
    {
        base.OnStepped();
        var coordList = TileManager.Instance.GetNeighborCoords(axialCoord);
        foreach (var coord in coordList)
        {
           TileManager.Instance.SteppedTile(coord);
        }
    }
}
