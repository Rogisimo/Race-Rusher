using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject gameOverScreen;
    public CarController playerController; // Reference to your player's control script

    private float startTime;
    private bool raceStarted = false;
    private bool raceFinished = false;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        playerController.canMove = false; // Disable movement

        timerText.text = "3";
        yield return new WaitForSeconds(1);

        timerText.text = "2";
        yield return new WaitForSeconds(1);

        timerText.text = "1";
        yield return new WaitForSeconds(1);

        timerText.text = "GO!";
        yield return new WaitForSeconds(1);

        timerText.text = "0.00";
        startTime = Time.time;
        raceStarted = true;
        playerController.canMove = true; // Enable movement
    }

    void Update()
    {
        if (raceStarted && !raceFinished)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");
            timerText.text = minutes + ":" + seconds;
        }
    }

    public void FinishRace()
    {
        raceFinished = true;
        gameOverScreen.SetActive(true); // Show game over screen
        playerController.canMove = false; // Disable movement again
        Time.timeScale = 0f;
    }

    public void RaceAgain(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
