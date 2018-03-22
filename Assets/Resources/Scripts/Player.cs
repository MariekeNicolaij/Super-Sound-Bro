using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rBody;

    [Range(100, 1000)]
    public float jumpForce = 750;
    [Range(100, 1000)]
    public float moveForce = 7.5f;
    [Range(0, 5)]
    public float holdSpeed = 5;
    float speed;
    float maxHorizontallyVelocity = 5;

    Vector2 startPosition;
    float defaultScale;

    bool isHolding;


    void Start()
    {
        speed = moveForce;
        defaultScale = transform.localScale.x;
        startPosition = transform.position;
        AudioManager.instance.PlayMusic(MusicType.Game);
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
        if (other.tag == "Finish")
            LevelComplete();
    }

    void InputCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
            Move();
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButton("Hold"))
            Hold(true);
        else if (Input.GetButtonUp("Hold"))
            Hold(false);
    }

    void Animations()
    {
        if (rBody.velocity.y > 0)
            animator.SetFloat("vSpeed", rBody.velocity.y);
        if (Input.GetAxis("Horizontal") != 0)
            animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        else
            animator.SetFloat("Speed", 0);
    }

    void FallCheck()
    {
        if (transform.position.y < -50)
            transform.position = startPosition;
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");

        transform.localScale = (h < 0) ? new Vector2(-defaultScale, defaultScale) : new Vector2(defaultScale, defaultScale);

        if (rBody.velocity.x <= maxHorizontallyVelocity && rBody.velocity.x >= -maxHorizontallyVelocity)
            rBody.AddForce(Vector2.right * h * speed);
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

    void Hold(bool hold)
    {
        isHolding = hold;
        animator.SetBool("Crouch", hold);

        speed = (isHolding) ? holdSpeed : moveForce;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        UIManager.instance.ShowGameOverPanel();
    }

    void LevelComplete()
    {
        Time.timeScale = 0;
        UIManager.instance.ShowLevelCompletePanel();
    }
}