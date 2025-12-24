using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject coin;
    [SerializeField] int maxCoinCount = 5;
    int currentCoinCount = 0;

    private void Start()
    {
        GameManager.Instance.startGameAction += StartAction;
    }

    private void OnDisable()
    {
        GameManager.Instance.startGameAction -= StartAction;
    }

    void StartAction()
    {
        StartCoroutine(SpawnCoinCor());

    }

    IEnumerator SpawnCoinCor()
    {
        while (true)
        {
            yield return null;
            if (currentCoinCount < maxCoinCount)
            {
                yield return new WaitForSeconds(5);
                SpawnCoin();
            }
        }
    }

    public void SpawnCoin()
    {
        Vector3 tilePos = TileManager.Instance.GetRandomTile().transform.position;

        GameObject coinObj = Instantiate(coin,tilePos + new Vector3(0,1f,0) , Quaternion.identity);
        coinObj.GetComponent<Coin>().disappearAction += DisappearCoin;
    }

    /// <summary>
    /// 코인이 사라질 때 행동 함수
    /// </summary>
    private void DisappearCoin()
    {
        currentCoinCount++;
    }
}
