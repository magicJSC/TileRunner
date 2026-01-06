using System.Collections;
using UnityEngine;

public class PlayerSign : MonoBehaviour
{
    [SerializeField] Transform playerSign;
    [SerializeField] Vector3 signPos;

    private void Start()
    {
        StartCoroutine(MoveCor());
    }

    IEnumerator MoveCor()
    {
        while (true)
        {
            yield return null;
            MoveSign();
        }
    }

    public void MoveSign()
    {
       Vector3 totalPos =  transform.position + signPos;
        playerSign.position = new Vector3(totalPos.x, signPos.y, totalPos.z);
    }
}
