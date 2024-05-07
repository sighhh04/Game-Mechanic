using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 5f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 0.1f;

    private Rigidbody2D rb;
    //private bool isDashing = false;
    private bool isGrounded = false;
    private bool canDash = true;
    private bool canDashInAir = false; //variable to control dashing in air

    private int dashType = 0; // 0 for regular dash, 1 for dash with IFrames, 2 for dash parry for projectiles
    private bool hasParry = false;
    public LayerMask dodgeLayer; // Layer mask for dodging
    public Text dashTypeText;

    public Text healthText;
    private int health = 5;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


        //invincibility dodge toggle
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (dashType == 0)
            {
                dashType = 1;
                dashTypeText.text = "Dash Type: Iframes";
            }
            else if (dashType == 1)
            {
                dashType = 2;
                dashTypeText.text = "Dash Type: Parry";
            }
            else
            {
                dashType = 0;
                dashTypeText.text = "Dash Type: Normal";
            }
        }
   

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            canDashInAir = true; // Reset canDashInAir when jumping
        }

        // Dashing
        if (Input.GetKeyDown(KeyCode.Mouse0) && (canDash || (!isGrounded && canDashInAir)))
        {
            StartCoroutine(Dash(moveInput));
           
        }

        if (health == 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Dash(float moveInput)
    {
        if (dashType == 1)
        {
            gameObject.layer = LayerMask.NameToLayer("Dodge");
        }
        if (dashType == 2) hasParry = true;
        
        canDash = false;
        canDashInAir = false;
        
        if (!isGrounded) canDash = false; // Prevent dashing on ground while in air
        float dashDirection = Mathf.Sign(moveInput);
        float startTime = Time.time;

        
        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = new Vector2(dashDirection * dashForce, rb.velocity.y);
            yield return null;
        } 
       
        
       
        hasParry = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        yield return new WaitForSeconds(dashCooldown);
        if (isGrounded) canDash = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDash = true; // Re-enable dashing on ground
        }

        if (collision.gameObject.CompareTag("Projectile"))
        {
         
            
             if (hasParry)
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    collision.gameObject.transform.rotation = Quaternion.Euler(collision.gameObject.transform.rotation.eulerAngles.x, collision.gameObject.transform.rotation.eulerAngles.x, -90);
                    rb.velocity = new Vector2(12, 0f); 
                }
            }
            else
            {
                Destroy(collision.gameObject);
                health--;
                healthText.text = "health: " + health;
            }
        }
    }

    

}
