using UnityEngine;

public class FogTile : HexTile
{
    [SerializeField] GameObject fogController;

    public override void OnStepped()
    {
        Instantiate(fogController, transform.position,Quaternion.identity);
        base.OnStepped();
    }
}
