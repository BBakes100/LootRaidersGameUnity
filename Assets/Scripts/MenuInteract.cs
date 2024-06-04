using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    MenuInteract Class handles functions for the Start Menu
*/
public class MenuInteract : MonoBehaviour
{
    /*
        Play Button
    */
    public void playGame(){
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1f;
    }

    /*
        Exit Button
    */
    public void exitGame(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
