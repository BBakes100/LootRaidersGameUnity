using System.Collections;
using UnityEngine;

/*
    Interface for interactable objects (treasure)
*/
public interface IInteractable
{
    void Interact();
}

/*
    Interactor class handles interacting behavior between
    the player and the chest to win the game
*/
public class Interactor : MonoBehaviour
{
    // Public Modifiers
    public Transform InteractorSource;
    public float InteractRange;
    public float holdTime = 1f;
    public GameObject treasure;

    // Private Variables
    private bool isInteracting = false; 

    /*
        Constantly Check for Interactions
    */
    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (!isInteracting)
            {
                // Animate Treasure Chest
                Animator treasureAnimator = treasure.GetComponent<Animator>();
                if (treasureAnimator != null)
                {
                    treasureAnimator.SetBool("IsOpening", true);
                    treasureAnimator.SetBool("IsClosing", false);
                }

                // Check Interaction key is being held
                StartCoroutine(HoldInteract());
            }
        }
        else
        {
            // Animate Treasure Chest
            Animator treasureAnimator = treasure.GetComponent<Animator>();
            if (treasureAnimator != null)
            {
                treasureAnimator.SetBool("IsClosing", true);
                treasureAnimator.SetBool("IsOpening", false);
            }

            // Stop interacting, player let go
            StopCoroutine(HoldInteract());
            isInteracting = false;
        }
    }

    /*
        Checks that the player is holding the interaction for as
        long as the timer requires.
    */
    private IEnumerator HoldInteract()
    {
        isInteracting = true;
        float timer = 0f;

        // Increment timer on hold
        while (timer < holdTime && Input.GetKey(KeyCode.E))
        {
            timer += Time.deltaTime;
            yield return null; 
        }

        // Check interact ray to call treasure's interact method.
        if (timer >= holdTime)
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                if (treasure.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }

        isInteracting = false;
    }
}
