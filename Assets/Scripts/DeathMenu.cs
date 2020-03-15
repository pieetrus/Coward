using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    public Image backgroundImg;

    private bool isShowned = false;
    private float transitionEndMenu = -1.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShowned)
            return;
        transitionEndMenu += Time.deltaTime;
        backgroundImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transitionEndMenu);
    }

    /// <summary>
    /// Showing EndMenu
    /// USED IN SCORE SCRIPT IN ONDEATH METHOD
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public IEnumerator ToggleEndMenu(float score)
    {
        yield return new WaitForSeconds(2); // wait for 2 seconds
        gameObject.SetActive(true);
        scoreText.text = ((int)score).ToString();
        isShowned = true;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //booting up again scene that we are in
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu"); //booting up Scene Menu
    }
}
