using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rBody;
    [Range(0, 10)]
    public float moveSpeed = 10;
    [Range(0, 5)]
    public float holdSpeed = 5;
    float speed;

    bool isHolding;

    void Start()
    {
        speed = moveSpeed;
    }

    void Update()
    {
        InputCheck();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("pensi");
        animator.SetBool("Ground", true);
    }

    void InputCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
            Move();
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Hold"))
            Hold();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");

        Debug.Log("Fix flip");
        transform.localScale = (h < 0) ? -Vector2.one : Vector2.one;

        animator.SetFloat("Speed", Mathf.Abs(h));
        transform.Translate(Vector2.right * h / speed);
    }

    bool CanJump()
    {
        Debug.Log("fix 0punt");
        return (rBody.velocity.y > 0) ? false : true;
    }

    void Jump()
    {
        // cannot jump again
        if(CanJump())
        rBody.AddForce(Vector2.up * 1000);
        animator.SetFloat("vSpeed", rBody.velocity.y);
    }

    void Hold()
    {
        isHolding = !isHolding;
        animator.SetBool("Crouch", isHolding);

        speed = (isHolding) ? holdSpeed : moveSpeed;
    }
}