using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastGun : MonoBehaviour
{
    // Components
    private Transform laserOrigin;
    private LineRenderer laserLine;

    // Public Modifiers
    public float gunRange = 50f;
    public float fireRate = 0.02f;
    public float laserDuration = 0.05f;
    public GameObject pauseMenu;

    // Private Variables
    private float fireTimer;
    private Vector3 previousPosition;

    /*
        Get Components and Initialize Variables
    */
    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        laserOrigin = transform;
        previousPosition = transform.position;
    }

    /*
        Continuously checks that the lasers are shot, and handles the raycast and
        renders it through the line renderer component.
    */
    void Update()
    {
        Vector3 currentPosition = transform.position;
        float speed = Vector3.Distance(previousPosition, currentPosition) / Time.deltaTime;
        previousPosition = currentPosition;
        fireTimer += Time.deltaTime;

        // Mouse 1 is pressed and pause isn't activated
        if (Input.GetButtonDown("Fire1") && fireTimer > fireRate && !pauseMenu.activeSelf)
        {
            fireTimer = 0;
            Vector3 rayOrigin = laserOrigin.position;

            // Check player movement speed so laser is in front of player
            if (speed > 5)
            {
                rayOrigin += transform.forward * 1.08f;
            }
            laserLine.SetPosition(0, rayOrigin);

            // Check whether raycast hit something
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, transform.forward, out hit, gunRange))
            {
                // Render laser and give enemy 1 damage
                laserLine.SetPosition(1, hit.point);
                EnemyMovement enemy = hit.collider.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
            else
            {
                // laser goes all the way until gun range is reached
                laserLine.SetPosition(1, rayOrigin + (transform.forward * gunRange));
            }
            StartCoroutine(ShootLaser());
        }
    }

    /*
        Render laser for a certain period of time.
    */
    IEnumerator ShootLaser(){
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
