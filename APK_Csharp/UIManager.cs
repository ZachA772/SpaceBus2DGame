using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject YouDiedUI; //UI panel shown when player dies
    [SerializeField] private GameObject DeathButtons; //Buttons shown after "You Died" UI

    [SerializeField] private GameObject pausePanel; //Pause menu UI panel
    private bool isPaused; //Tracks whether the game is currently paused

    //Called when the "Start Game" button is pressed
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); //Load first level
        if (pausePanel != null) pausePanel.SetActive(false); //Hide pause UI if active
    }

    private void Update()
    {
        //Check for Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    //Called when the "Back to Menu" button is pressed
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); //Load main menu scene
        Time.timeScale = 1f; //Ensure time scale is normal/reset
    }

    //Called when the "Exit Game" button is pressed
    public void ExitGame()
    {
        Application.Quit(); //Close the application
    }

    //Hides start menu UI and restarts level
    public void RestartButton()
    {
        SceneManager.LoadScene("Level1"); //Reload Level1
    }

    public void OnPlayerDeath()
    {
        YouDiedUI.SetActive(true); //Show "You Died" panel
        StartCoroutine(ShowYouDiedAfterDelay(2f)); //Show buttons after delay
    }

    private IEnumerator ShowYouDiedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); //Wait before showing buttons
        DeathButtons.SetActive(true); //Enable death buttons UI
    }

    public void TogglePause()
    {
        isPaused = !isPaused; //Toggle pause state

        if (pausePanel != null) pausePanel.SetActive(isPaused); //Show/hide pause panel
        Time.timeScale = isPaused ? 0f : 1f; //Freeze or resume game time
    }

    public void Resume()
    {
        isPaused = false; //Unpause
        if (pausePanel != null) pausePanel.SetActive(false); //Hide pause panel
        Time.timeScale = 1f; //Resume game time
    }
}
