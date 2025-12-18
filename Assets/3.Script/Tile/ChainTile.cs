using UnityEngine;

public class ChainTile : SpecialTile
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

    public override void TimerOver()
    {
        OnStepped();
    }
}
