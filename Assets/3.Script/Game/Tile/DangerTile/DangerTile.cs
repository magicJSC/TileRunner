using UnityEngine;

public class DangerTile : SpecialTile
{
    public override void OnStepped()
    {
        if (curTime <= 0 || isDisappear) return;

        base.OnStepped();
        DangerManager.Instance.Execute(transform);
    }

}
