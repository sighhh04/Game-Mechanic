using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public float spawnInterval = 3f; // Time interval between each spawn
    public float projectileSpeed = -7;

    private float timer = 0f;

    void Update()
    {
        // Increment the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a projectile
        if (timer >= spawnInterval)
        {
            // Spawn the projectile
            SpawnProjectile();
            // Reset the timer
            timer = 0f;
        }
    }

    void SpawnProjectile()
    {
        // Instantiate the projectile at the spawn point
        GameObject newProjectile = Instantiate(projectilePrefab);
        newProjectile.transform.localPosition = transform.localPosition;
        // Optionally, you can set the projectile's velocity or direction here
        // For simplicity, let's assume it moves to the left
        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(projectileSpeed, 0f); // Example velocity to the left
        }

    }

    
}
