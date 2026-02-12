using System.Collections;
using UnityEngine;

public class PlayerSign : MonoBehaviour
{
    [SerializeField] Transform playerSign;
    [SerializeField] Vector3 signPos;

    private void Start()
    {
        playerSign.parent = null;

        StartCoroutine(MoveCor());
    }

    private void OnDestroy()
    {
        if(playerSign != null)
             Destroy(playerSign.gameObject);
    }

    IEnumerator MoveCor()
    {
        while (true)
        {
            yield return null;
            MoveSign();
        }
    }

    private void MoveSign()
    {
        if(playerSign == null)
            return;

        Vector3 totalPos =  transform.position + signPos;
        playerSign.position = new Vector3(totalPos.x, signPos.y, totalPos.z);
    }
    
}
