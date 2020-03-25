using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private float score = 0.0f;
    
    [SerializeField]
    private TextMeshProUGUI scoreText = null;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;

    private bool isDead = false;

    public DeathMenu deathMenu;

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
        if(PlayerPrefs.GetFloat("Highscore") < score)
            PlayerPrefs.SetFloat("Highscore", score);

        StartCoroutine(deathMenu.ToggleEndMenu(score));
    }
}
