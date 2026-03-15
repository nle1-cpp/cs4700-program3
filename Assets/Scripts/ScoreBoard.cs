using UnityEngine;
using TMPro;
public class ScoreBoard : MonoBehaviour
{
	public TMP_Text scoreText; // Reference to the UI text element
	public int score; // The actual score value

	void Start()
	{
		score = 0; // Initialize score to zero
		UpdateScoreText(); // Display the initial score
	}

	public void AddScore(int points)
	{
		score += points; // Add points to the score
		UpdateScoreText(); // Update the UI display
	}

	public void ResetScore()
	{
		score = 0;
		UpdateScoreText(); // Update the UI display
	}

	void UpdateScoreText()
	{
		scoreText.text = "Score: \n" + score.ToString(); // Format and display the score
	}

}
