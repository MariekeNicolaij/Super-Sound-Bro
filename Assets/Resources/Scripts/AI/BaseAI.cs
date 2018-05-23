using UnityEngine;

public class BaseAI : MonoBehaviour
{
    public Player player;
    public Rigidbody2D rBody;
    [HideInInspector]
    public StateManager stateManager;       // Handles the states

    public Vector2 direction = Vector2.right;

    public ParticleSystem dieParticleSystem;

    [Range(0, 10)]
    public float speed = 3;
    public bool facingRight = true;

    // State timers
    float stateTime;
    float minIdleTime = 1, maxIdleTime = 3, minWanderTime = 4, maxWanderTime = 8;


    void Start()
    {
        // If player has not been set it will search for the player
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (!rBody)
            rBody = GetComponent<Rigidbody2D>();

        if (!dieParticleSystem)
            dieParticleSystem = GetComponentInChildren<ParticleSystem>();

        stateManager = new StateManager(this, new IdleState());     // Handles the states
        stateTime = Random.Range(minIdleTime, maxIdleTime);
    }

    void Update()
    {
        stateManager.Execute();                          // Update stateManager
        StateTimer();
        WallCheck();
    }

    void StateTimer()
    {
        if (stateTime > 0)
            stateTime -= Time.deltaTime;
        else if (stateManager.currentState is IdleState)
        {
            stateTime = Random.Range(minWanderTime, maxWanderTime);
            stateManager.SwitchState(new WanderState());
        }
        else if (stateManager.currentState is WanderState)
        {
            stateTime = Random.Range(minIdleTime, maxIdleTime);
            stateManager.SwitchState(new IdleState());
        }
    }

    void StartDieAnimation()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Weapon")
            stateManager.SwitchState(new DieState());
        if(other.transform.tag == "Obstacle")
        {
            Debug.Log("Hit");
            Flip();
            direction = OppositeDirection();
        }
    }

    void WallCheck()
    {

    }

    public void Flip()
    {
        facingRight = !facingRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;                           // Flips scale
        transform.localScale = newScale;
    }

    /// <summary>
    /// Either left or right
    /// </summary>
    /// <returns></returns>
    public Vector2 RandomDirection()
    {
        int chanceFactor = Random.Range(0, 100);
        int chance = 50; // %

        rBody.velocity = Vector2.zero; // So it doesnt slide

        return (chanceFactor < chance) ? Vector2.right : Vector2.left;
    }

    /// <summary>
    /// Used when hitting a wall or structure to change its direction
    /// </summary>
    /// <returns></returns>
    Vector2 OppositeDirection()
    {
        return (direction == Vector2.right) ? Vector2.left : Vector2.right;
    }
}