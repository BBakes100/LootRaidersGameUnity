using UnityEngine;

/*
    EnemyMovement class responsible for handling all
    enemy behavior including movement, switching animations,
    health, and attacking.
*/
public class EnemyMovement : MonoBehaviour
{
    // Components
    private Transform player;
    private Rigidbody rb;
    private Animator animator;

    // Public Modifiers
    public GameObject playerObj;
    public float moveSpeed = 3f;
    public float currentHealth = 10f;
    public float detectionRadius = 10f;
    public float rotationSpeed = 5f;
    
    // Private Variables
    private float minDistanceToPlayer = 0.8f;
    private bool dead = false;

    /*
        Get Components from Inspector
    */
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    /*
        Enemy Movement, Rotation, and Animations
    */
    void Update()
    {
        if (player != null && !dead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Check enemy is between a certain distance from player to keep from glitching inside
            if (distanceToPlayer <= detectionRadius && distanceToPlayer > minDistanceToPlayer)
            {
                Vector3 direction = (player.position - transform.position).normalized;

                // Move and Rotate Towards Player
                rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
                animator.SetBool("IsWalking", true);
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

                // Attack
                if (distanceToPlayer < 1)
                {
                    animator.SetTrigger("Attack");
                }
            
            // Outside player distance, no more walking
            } else {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    /*
        Check collisions with player and damage if colliding
    */
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<playerMovement>().TakeDamage(1);
        }
    }

    /*
        Take damage from the player until 0 health
    */
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        animator.SetBool("IsWalking", false);
        currentHealth -= damage;

        // Enemy death animation
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            dead = true;
            Invoke("Die", 4f);
        }
    }

    /*
        Remove enemy from scene
    */
    void Die()
    {
        Destroy(gameObject);
    }
}
