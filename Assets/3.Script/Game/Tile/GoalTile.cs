using UnityEngine;

public class GoalTile : HexTile
{
  public override void OnStepped()
  {
        base.OnStepped();
        TileManager.Instance.OnReachGoal(axialCoord);

        GameManager.Instance.Score += 50;
    }
}
