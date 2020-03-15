using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private float score = 0.0f;
    
    [SerializeField]
    private Text scoreText = null;

    private int difficultyLevel = 1;
    private static int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;

    private bool isDead = false;

    void Update()
    {
        if (isDead)
            return;
        
        if (score >= scoreToNextLevel)
            LevelUp();
        
        score += Time.deltaTime * difficultyLevel;
        scoreText.text = ((int)score).ToString();
    }


    void LevelUp()
    {
        if (difficultyLevel == maxDifficultyLevel)
            return;

        scoreToNextLevel *= 2;
        difficultyLevel++;

        GetComponent<PlayerMotor>().SetSpeed(difficultyLevel);
    }

    /// <summary>
    /// Used in PlayerMotor script
    /// </summary>
    public void OnDeath()
    {
        isDead = true;
    }
}
