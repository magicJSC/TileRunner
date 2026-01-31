using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinTile : HexTile
{
    [SerializeField] GameObject coin;
    [SerializeField] GameObject effect;
    [SerializeField] AudioClip coinSound;

    public override void OnStepped()
    {
        base.OnStepped();
        GameManager.Instance.Coin++;
        Instantiate(effect, coin.transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX(coinSound);
        coin.SetActive(false);
    }
}
