using TMPro;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText = Util.FindChild<TextMeshProUGUI>(gameObject, "Score");

        GameManager.Instance.scoreAction += UpdateScore;
        UpdateScore(GameManager.Instance.Score);
    }

    private void OnDisable()
    {
        GameManager.Instance.scoreAction -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"{score}";
    }
}
