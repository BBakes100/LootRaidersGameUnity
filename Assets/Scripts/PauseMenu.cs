using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    PauseMenu Class handles functions for the pause pop up
    as well as losing pop up.
*/
public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject loseMenuUI;

    void Update()
    {
        // Check pause is pressed
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gameIsPaused){
                resume();
            } else {
                pause();
            }
        }
    }

    /*
        Resume Button
    */
    public void resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    /*
        Pauses Game
    */
    void pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    /*
        Back to Menu Button
    */
    public void backToMenu(){
        SceneManager.LoadSceneAsync(0);
    }

    /*
        Exit Game Button
    */
    public void exitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /*
        Retry Button
    */
    public void retry(){
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1f;

    }
}
