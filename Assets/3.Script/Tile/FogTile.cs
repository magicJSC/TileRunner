using UnityEngine;

public class FogTile : SpecialTile
{
    [SerializeField] GameObject fogController;

    public override void OnStepped()
    {
        Instantiate(fogController, transform.position,Quaternion.identity);
        base.OnStepped();
    }
    public override void TimerOver()
    {
        OnStepped();
    }
}
