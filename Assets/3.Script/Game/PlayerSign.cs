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
        GameManager.Instance.endGameAction += Dissapear;
    }

    private void OnDisable()
    {
        GameManager.Instance.endGameAction -= Dissapear;
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
    
    void Dissapear()
    {
        Destroy(playerSign.gameObject);
    }
}
