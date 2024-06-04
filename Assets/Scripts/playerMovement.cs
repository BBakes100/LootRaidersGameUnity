using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    PlayerMovement class responsible for handling all
    player behavior including movement, switching animations,
    health, and attacking.
*/
public class playerMovement : MonoBehaviour
{
    // Components
    private CharacterController controller;
    private Transform playerTransform;
    private Animator animator;

    // Public Modifiers
    public GameObject loseMenuUI;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 2.0f;
    public float gravityValue = -9.81f;
    public float turnSmoothTime = 0.1f;
    public float damageCooldown = 0f;
    public int currentHealth = 10;

    // Private Variables
    private float turnSmoothVelocity;
    private bool canTakeDamage = true;
    private bool isGrounded = true;
    private float lastDamageTime;
    private Vector3 playerVelocity;

    /*
        Get Components and Initialize Variables
    */
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
        lastDamageTime = -damageCooldown;
        animator = GetComponent<Animator>();
    }

    /*
        Player Movement, Rotation, and Animations
    */
    void Update()
    {
        if(controller != null){

            // Player Can Take Damage
            if (!canTakeDamage && Time.time - lastDamageTime >= damageCooldown)
            {
                canTakeDamage = true;
            }

            // Player Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
                animator.SetTrigger("IsJumping");
                isGrounded = false;
            }

            // Player Movement
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            // Sprinting
            if (isRunning) {
                controller.Move(move * Time.deltaTime * (playerSpeed * 2));
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsRunning", true);

            // Walking
            } else {
                if (move != Vector3.zero) {
                    controller.Move(move * Time.deltaTime * playerSpeed);
                    animator.SetBool("IsWalking", true);
                    animator.SetBool("IsRunning", false);
                } else {
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsRunning", false);
                }
            }

            // Player Rotation
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);
        
            // Player Gravity Modification
            if(playerVelocity.y <= -3){
                playerVelocity.y += 0;
            } else if (playerVelocity.y >= 7){
                playerVelocity.y += (gravityValue * Time.deltaTime) - 1;
            } else {
                playerVelocity.y += gravityValue * Time.deltaTime;
            }

            // Move Player Again
            if(controller != null){
                controller.Move(playerVelocity * Time.deltaTime);
            }
            
        }
    }

    /*
        Check Player Collisions
    */
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        } else if (hit.gameObject.CompareTag("Enemy") && canTakeDamage)
        {
            TakeDamage(1);
            lastDamageTime = Time.time; 
            canTakeDamage = false;
        } else if (hit.gameObject.CompareTag("Treasure"))
        {
            Debug.Log("treasure");
        }
    }

    /*
        Modifies Player Health
    */
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /*
        Ends the Game
    */
    void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
