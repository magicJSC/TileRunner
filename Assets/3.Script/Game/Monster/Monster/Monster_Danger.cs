using UnityEngine;

public class Monster_Danger : Monster_Chase
{
    [SerializeField] int dangerTileCount = 2;

    public override void Die()
    {
        base.Die();
        for (int i = 0; i < dangerTileCount; i++)
        {
            TileManager.Instance.RandomTileChangeDanger();
        }
    }
}
