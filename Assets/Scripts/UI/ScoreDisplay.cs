using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    //[SerializeField] private int currentScore = 0; (REPLACED by GameManager.score)

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    //Added by ZS to update score and ensure that score gets added to the disabled score tracker once the object is enabled
    private void OnEnable() 
    { 
        UpdateScoreText(); 
    }

    // ScoreManager.cs now updates a static variable in GameManager rather than calling this function
    //public void UpdateScore(int score)
    //{
    //    currentScore = score; 
    //    UpdateScoreText();
    //    //text.text = "Score: " + score;
    //}

    public void UpdateScoreText()
    {
        if (text != null)
        {
            text.text = "Score: " + GameManager.score;
        }
        else { Debug.LogError("No score text detected"); }
    }
}
