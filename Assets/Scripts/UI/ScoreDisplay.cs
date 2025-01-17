using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int currentScore = 0;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    //Added by ZS to update score and ensure that score gets added to the disabled score tracker once the object is enabled
    private void OnEnable() 
    { 
        UpdateScoreText(); 
    }
    public void UpdateScore(int score)
    {
        currentScore = score; 
        UpdateScoreText();
        //text.text = "Score: " + score;
    }
    private void UpdateScoreText()
    {
        if (text != null)
        {
            text.text = "Score: " + currentScore;
        }
        else { Debug.LogError("No score text detected"); }
    }
}
