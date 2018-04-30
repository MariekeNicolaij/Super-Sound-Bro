using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rBody;

    GameObject currentPlug;

    [Range(100, 2000)]
    public float jumpForce = 1000;
    [Range(0, 10)]
    public float moveSpeed = 7.5f;
    [Range(0, 5)]
    public float holdSpeed = 3.75f;
    float speed;

    Vector2 startPosition;
    float defaultScale;

    public bool isHolding;
    bool canHold;
    bool facingRight = true;

    bool pause;


    void Start()
    {
        speed = moveSpeed;
        defaultScale = transform.localScale.x;
        startPosition = transform.position;
        //AudioManager.instance.PlayMusic(MusicType.Game);
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
                currentPlug = other.gameObject;
            }
        }
        if (other.tag == "Finish")
            LevelComplete();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Plug")
        {
            canHold = false;
            Hold(false);
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
        if (Input.GetButtonDown("Hold"))
            Hold(true);
        if (Input.GetButtonUp("Hold"))
            Hold(false);
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
        return (rBody.velocity.y <= 0 && animator.GetBool("CanJump")) ? true : false;
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
        if (!canHold && hold && !currentPlug)   // If you are close enough to hold and if you currently are not holding anything then go
            return;

        speed = (hold) ? holdSpeed : moveSpeed; // Change player speed
        animator.SetBool("Hold", hold);         // Change the animation according to when player is holding or not

        RaycastHit2D hit;
        HingeJoint2D hingeJoint2D = new HingeJoint2D();

        if (currentPlug) // Plug is set in Trigger
        {
            hit = (facingRight) ? Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity) : Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity);

            Debug.Log(hit.point);

            if (!currentPlug.GetComponent<HingeJoint2D>())
                currentPlug.AddComponent<HingeJoint2D>();
            hingeJoint2D = currentPlug.GetComponent<HingeJoint2D>();
            hingeJoint2D.connectedBody = rBody;
            //hingeJoint2D.connectedAnchor = hit.point.normalized;

            //if (currentPlug.GetComponent<Rigidbody2D>())
            //    currentPlug.GetComponent<Rigidbody2D>().isKinematic = (hold) ? true : false;
            //currentPlug.transform.parent = (hold) ? transform : null;
        }
        else if (hingeJoint2D)
            hingeJoint2D.connectedBody = null;

        isHolding = hold;
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