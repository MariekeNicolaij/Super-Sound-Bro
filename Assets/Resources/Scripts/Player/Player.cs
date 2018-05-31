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
    Weapon currentWeapon;

    [Range(100, 1000)]
    public float throwForce = 750;
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

    bool pause, gameOver;


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
        if (!isHolding)
        {
            if (other.tag == "Plug")
            {
                canHold = true;
                currentPlug = other.gameObject.GetComponent<Plug>();
            }
            if (other.tag == "Weapon")
            {
                canHold = true;
                currentWeapon = other.gameObject.GetComponent<Weapon>();
            }
        }
        if (other.tag == "Finish")
            LevelComplete();
        if (other.tag == "Enemy")
            GameOver();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isHolding && currentPlug)
        {
            if (other.tag == "Plug")
            {
                Hold(false);
                canHold = false;
                currentPlug = null;
            }
            if (other.tag == "Weapon")
            {
                Hold(false);
                canHold = false;
                currentWeapon = null;
            }
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

    public void Pause()
    {
        if (gameOver)
            return;

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

        if (!canHold)   // If you are close enough to hold and if plug and weapon exists (Plug and weapon are set in Trigger) then go
            return;

        if (currentPlug)
            ToggleHoldPlug(hold);
        else if (currentWeapon)
            ToggleHoldWeapon(hold);
    }

    void ToggleHoldPlug(bool hold)
    {
        if (hold)
            aimPosRaw = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * 4)); // 4 = multiplier
        aimLineRenderer.enabled = hold;
        currentPlug.ToggleWalkThroughPlug(hold);
        currentPlug.joint2D.enabled = hold;
    }

    void ToggleHoldWeapon(bool hold)
    {
        if (hold)
            aimPosRaw = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * 4)); // 4 = multiplier
        aimLineRenderer.enabled = hold;
        currentWeapon.ToggleWalkThroughWeapon(hold);
        currentWeapon.joint2D.enabled = hold;
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
        if (!isHolding)
            return;
        Hold(false);

        if (currentPlug)
        {
            currentPlug.rBody.AddForce(ThrowDirection() * ActualThrowForce());
            currentPlug = null;
        }
        else if (currentWeapon)
        {
            currentWeapon.rBody.AddForce(ThrowDirection() * ActualThrowForce());
            StartCoroutine(WeaponLerpDelay(1));
        }
    }

    IEnumerator WeaponLerpDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        currentWeapon.lerpToPlayer = true;
        currentWeapon.rBody.gravityScale = 0;
        currentWeapon.rBody.velocity = Vector2.zero;
        currentWeapon = null;
    }

    /// <summary>
    /// Normalized
    /// </summary>
    /// <returns></returns>
    Vector3 ThrowDirection()
    {
        return -(transform.position - aimPosWorld).normalized;
    }

    /// <summary>
    /// Multiplies specified throwforce with the throw distance (aim line)
    /// </summary>
    /// <returns></returns>
    float ActualThrowForce()
    {
        float distance = Vector2.Distance(transform.position, aimPosWorld);
        return distance * throwForce;
    }

    public void GameOver()
    {
        gameOver = true;
        Time.timeScale = 0;
        UIManager.instance.ToggleGameOverPanel(true);
        AudioManager.instance.PlaySound(SoundType.Death);
    }

    void LevelComplete()
    {
        Time.timeScale = 0;
        UIManager.instance.ToggleLevelCompletePanel(true);
        AudioManager.instance.PlaySound(SoundType.Victory);
    }
}