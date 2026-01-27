using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinTile : HexTile
{
    [SerializeField] GameObject coin;

    public override void OnStepped()
    {
        base.OnStepped();
        GameManager.Instance.Coin++;
        coin.SetActive(false);
    }
}
