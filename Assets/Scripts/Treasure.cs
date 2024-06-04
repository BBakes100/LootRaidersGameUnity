using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Treasure class implements interactable interface
    for game flow.
*/
public class Treasure : MonoBehaviour, IInteractable
{
    /*
        Loads win screen if interacted with.
    */
    public void Interact(){
        SceneManager.LoadSceneAsync(2);
    }
}
