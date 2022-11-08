using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;

    public float speed = 1;

    private Animator anim;
    private Rigidbody2D rb;

    private Vector2 lookDir;
    private Vector2 deltaPosition = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Runs every frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        // if we are allowed to move and there was movement input
        if (canMove && (!Mathf.Approximately(x, 0) || !Mathf.Approximately(y, 0)))
        {
            // Direction of motion
            lookDir = new Vector2(x, y);
            lookDir.Normalize();

            // Tell the animator component which direction we are facing so it plays
            // the correct animation
            anim.SetFloat("Look X", lookDir.x);
            anim.SetFloat("Look Y", lookDir.y);

            // Tell the animator component to play the moving animation
            anim.SetBool("Moving", true);

            // Calculate change in position based on direction, speed, and time elapsed since the last frame
            deltaPosition += lookDir * speed * Time.deltaTime;
        }
        else
        {
            // if the player can't move, play the idle animation 
            anim.SetBool("Moving", false);
        }

    }

    // Runs exactly 50 times per second, regardless of variations in framerate
    // Used to ensure player moves at a set speed
    void FixedUpdate()
    {
        // Update player position
        rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + deltaPosition);
        deltaPosition = new Vector2();
    }
}
