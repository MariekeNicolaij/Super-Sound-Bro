using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rBody;
    LineRenderer aimLineRenderer;
    Vector3 aimPosRaw, aimPosWorld;

    Plug currentPlug;

    [Range(10, 100)]
    public float throwForce = 75;
    [Range(100, 2000)]
    public float jumpForce = 1000;
    [Range(0, 10)]
    public float moveSpeed = 7.5f;
    [Range(0, 5)]
    public float holdSpeed = 3.75f;
    float speed;
    float aimSpeed = 10;

    Vector2 startPosition;
    float defaultScale;

    public bool isHolding;
    bool canHold;
    bool facingRight = true;

    bool pause;


    void Start()
    {
        aimLineRenderer = GetComponent<LineRenderer>();
        aimLineRenderer.enabled = false;

        speed = moveSpeed;
        defaultScale = transform.localScale.x;
        startPosition = transform.position;
    }

    void Update()
    {
        InputCheck();
        Animations();
        FallCheck();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        animator.SetBool("CanJump", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Plug")
        {
            if (!isHolding)
            {
                canHold = true;
                currentPlug = other.gameObject.GetComponent<Plug>();
            }
        }
        if (other.tag == "Finish")
            LevelComplete();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isHolding)
            if (other.tag == "Plug")
            {
                canHold = false;
                currentPlug = null;
            }
    }

    void InputCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
            Move();
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Pause"))
            Pause();
        if (Input.GetButtonDown("Throw"))
            Throw();
        if (Input.GetButtonDown("Hold"))
            if (canHold)
                Hold(!isHolding);
        if (isHolding)
            Aim();
    }

    void Animations()
    {
        animator.SetFloat("vSpeed", rBody.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    void FallCheck()
    {
        if (transform.position.y < -50)
        {
            transform.position = startPosition;
            rBody.velocity = Vector2.zero;
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");

        if (!isHolding)
        {
            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();
        }
        rBody.velocity = new Vector2(h * speed, rBody.velocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    bool CanJump()
    {
        return (rBody.velocity.y <= 0 && animator.GetBool("CanJump") && !isHolding) ? true : false;
    }

    void Jump()
    {
        if (CanJump())
            rBody.AddForce(Vector2.up * jumpForce);
        animator.SetBool("CanJump", false);
    }

    void Pause()
    {
        pause = !pause;
        Time.timeScale = System.Convert.ToInt32(!pause);
        UIManager.instance.TogglePausePanel(pause);
        if (!pause)
            UIManager.instance.title.text = "";
    }

    public void Hold(bool hold)
    {
        if (!CanJump() && !isHolding)     // We don't want the player to pickup the plug mid-air
            return;

        speed = (hold) ? holdSpeed : moveSpeed; // Change player speed
        animator.SetBool("Hold", hold);         // Change the animation according to when player is holding or not

        isHolding = hold;

        if (!canHold && !currentPlug)   // If you are close enough to hold and if plug exists (Plug is set in Trigger) then go
            return;

        if (hold) // If you want to hold it
        {
            aimLineRenderer.enabled = true;
            aimPosRaw = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * 4)); // 4 = multiplier
            //currentPlug.ToggleWalkThroughPlug(false);
            currentPlug.joint2D.enabled = true;
        }
        else     // "Resets plug"
        {
            aimLineRenderer.enabled = false;
            //currentPlug.ToggleWalkThroughPlug(true);
            currentPlug.joint2D.enabled = false;
        }
    }

    void UpdateAimPosInWorldSpace()
    {
        if (Input.GetJoystickNames().Length == 0)       // Listens to mouse input
            aimPosRaw = Input.mousePosition;
        else                                            // Listens to controller input
            aimPosRaw += new Vector3(Input.GetAxis("MouseX") * -aimSpeed, Input.GetAxis("MouseY") * aimSpeed);

        aimPosRaw.z = -Camera.main.transform.position.z;     // Set opposite depth of camera so that eventually the line follows cursor
        aimPosWorld = Camera.main.ScreenToWorldPoint(aimPosRaw); // Converts mouse/analog position to world position
    }

    void Aim()
    {
        UpdateAimPosInWorldSpace();
        animator.SetBool("CanJump", true);  // When set to false it thinks its 'falling' so it puts up the 'falling animations'. This actually prevents the player from jumping and that is what we want

        aimLineRenderer.SetPosition(0, transform.position); // Start at player
        aimLineRenderer.SetPosition(1, aimPosWorld);    // Set position to cursor position in world space
    }

    void Throw()
    {
        if (!currentPlug)
            return;
        Hold(false);

        currentPlug.rBody.AddForce(ThrowDirection() * ActualThrowForce());

        currentPlug = null;
    }

    /// <summary>
    /// Normalized
    /// </summary>
    /// <returns></returns>
    Vector3 ThrowDirection()
    {
        return -(transform.position - aimPosWorld).normalized;
    }

    float ActualThrowForce()
    {
        float distance = Vector2.Distance(transform.position, aimPosWorld);
        return distance * throwForce;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        UIManager.instance.ToggleGameOverPanel(true);
    }

    void LevelComplete()
    {
        Time.timeScale = 0;
        UIManager.instance.ToggleLevelCompletePanel(true);
    }
}