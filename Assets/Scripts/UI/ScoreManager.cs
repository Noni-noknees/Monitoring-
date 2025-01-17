using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMPro.TMP_Text scoreText;

    void Start()
    {
        UpdateScoreText(); // Initialize the score text at the start
    }

    public void AddScore(int points)
    {
        score += points; // Add points to the score
        UpdateScoreText(); // Update the displayed score
    }

    void UpdateScoreText()
    {
        scoreText.text = "Keys: " + score; // Update the score display
    }
}

