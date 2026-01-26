using UnityEngine;

public class GoalTile : HexTile
{
  public override void OnStepped()
  {
        base.OnStepped();
        TileManager.Instance.OnReachGoal(axialCoord);
        GameManager.Instance.Player.transform.position = new Vector3(transform.position.x,GameManager.Instance.Player.transform.position.y,transform.position.z);
    }
}
