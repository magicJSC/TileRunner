using System.Collections;
using UnityEngine;

public class UI_DoubleScore : MonoBehaviour
{
    [SerializeField] float doubleTime = 5f;
    float curTime = 0;

    private GameObject doubleText;
    public void Start()
    {
        doubleText = Util.FindChild(gameObject, "DoubleText");
        doubleText.SetActive(false);
        GameManager.Instance.doubleScoreAction += StartDouble;
    }

    private void OnDisable()
    {
        GameManager.Instance.doubleScoreAction -= StartDouble;
        GameManager.Instance.isIncreaseDoubleScore = false;
    }

    private void StartDouble()
    {
        curTime = 0;
        doubleText.SetActive(true);

        if (curTime <= 0)
            StartCoroutine(DoubleCor());
    }

    /// <summary>
    /// 점수 2배 코루틴
    /// </summary>
    private IEnumerator DoubleCor()
    {
        GameManager.Instance.isIncreaseDoubleScore = true;

        while (curTime < doubleTime)
        {
            curTime += Time.deltaTime;
            doubleText.SetActive(true);
            yield return null;
        }
        EndDouble();
    }

    private void EndDouble()
    {
        GameManager.Instance.isIncreaseDoubleScore = false;
        doubleText.SetActive(false);
    }
}
