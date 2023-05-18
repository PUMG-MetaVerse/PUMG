using UnityEngine;
using TMPro;

public class TargetScoreManager : MonoBehaviour
{
    public static TargetScoreManager Instance;

    public int count = 0;
    public TMP_Text HitText;
    public TMP_Text ResultHitText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        count += score;
        UpdateHitText();
        UpdateResultHitText();
    }

    private void UpdateResultHitText()
    {
        ResultHitText.text = "획득 점수 : " + count.ToString() + "점";
    }

    private void UpdateHitText()
    {
        HitText.text = "획득 점수 : " + count.ToString() + "점";
    }
}
