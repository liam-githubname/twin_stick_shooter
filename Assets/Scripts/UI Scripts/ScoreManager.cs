using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//There is single score manager in the scene you must add a score = FindObjectOfType<ScoreManager>() to access it
//TODO: there is an issue with the shotgun blast causing it to update scoreMultiplier more than once for one enemy
public class ScoreManager : MonoBehaviour
{
  // Start is called before the first frame update
  public TMP_Text scoreText;
  public TMP_Text highscoreText;
  public TMP_Text scoreMultiplierText;
  [SerializeField] int scoreMultiplier = 1;
  public int score = 0;
  int highscore = 0;
  void Start()
  {
    highscore = PlayerPrefs.GetInt("highscore", 0);
    scoreText.text = "SCORE: " + score.ToString();
    scoreMultiplierText.text = "MULTIPLIER: " + scoreMultiplier.ToString();
    highscoreText.text = "HIGHSCORE: " + highscore.ToString();
  }


  public void Update()
  {
    scoreText.text = "SCORE: " + score.ToString();
    scoreMultiplierText.text = "MULTIPLIER: " + scoreMultiplier.ToString();
    highscore = (score > highscore) ? score : highscore;
    highscoreText.text = "HIGHSCORE: " + highscore.ToString();
  }

  public void updateScoreMultiplier(bool takenDamage)
  {
    if (takenDamage)
    {
      scoreMultiplier = 1;
    }
    else
    {
      scoreMultiplier++;
    }
  }

  public void updateScore(int scoreToAdd)
  {
    score += scoreToAdd * scoreMultiplier;
  }


}
