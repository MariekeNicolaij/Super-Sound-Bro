using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rBody;
    public DistanceJoint2D joint2D;
    GameObject imageObject;

    Player player;
    Vector2 startPosition;

    public bool lerpToPlayer;
    [Range(1, 5)]
    public float boomerngSpeed = 2.5f;


    void Start()
    {
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (!joint2D)
            joint2D = (!GetComponent<DistanceJoint2D>()) ? gameObject.AddComponent<DistanceJoint2D>() : GetComponent<DistanceJoint2D>();
        joint2D.connectedBody = player.rBody;
        joint2D.enabled = false;

        imageObject = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    void Update()
    {
        FallCheck();
        if (lerpToPlayer)
            LerpBackToPlayer();
    }

    void FallCheck()
    {
        if (transform.position.y < -50)
        {
            transform.position = startPosition;
            rBody.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            rBody.gravityScale = 1;
            lerpToPlayer = false;
        }
    }

    public IEnumerator WeaponLerpDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        lerpToPlayer = true;
        rBody.gravityScale = 0;
        rBody.velocity = Vector2.zero;
        player.currentWeapon = null;
    }

    public void LerpBackToPlayer()
    {
        transform.position = Vector2.Lerp(transform.position, player.transform.position, Time.deltaTime * boomerngSpeed);
    }

    public void ToggleWalkThroughWeapon(bool canWalkThroughWeapon)
    {
        if (!canWalkThroughWeapon)
            StartCoroutine(EnableWalkThroughWeaponDelay(0.5f));      // Delay
        else
            imageObject.layer = 10;   // 0 = Default, 10 = Weapon. In physics matrix player cant collide with weapon
    }

    /// <summary>
    /// Prevents the weapon from hitting the player first
    /// </summary>
    /// <returns></returns>
    IEnumerator EnableWalkThroughWeaponDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        imageObject.layer = 0;   // 0 = Default, 10 = Weapon. In physics matrix player cant collide with weapon
    }
}
