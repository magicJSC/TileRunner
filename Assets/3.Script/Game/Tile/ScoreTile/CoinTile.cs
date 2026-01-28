using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinTile : HexTile
{
    [SerializeField] GameObject coin;
    [SerializeField] GameObject effect;

    public override void OnStepped()
    {
        base.OnStepped();
        GameManager.Instance.Coin++;
        Instantiate(effect, coin.transform.position, Quaternion.identity);
        coin.SetActive(false);
    }
}
