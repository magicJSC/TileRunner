using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] GameObject meteor;
    [SerializeField] float spawnHeight;
    [SerializeField] int spawnCount;

    private void Start()
    {
        SpawnMeteor();
    }

    void SpawnMeteor()
    {
        var coordList = TileManager.Instance.GetRandomActivateCoord(spawnCount);
        
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject go = Instantiate(meteor);

            go.transform.position = new Vector3(0,spawnHeight,0)
                + TileManager.Instance.GetTile(coordList[i]).transform.position;
        }

        Destroy(gameObject);
    }
}
