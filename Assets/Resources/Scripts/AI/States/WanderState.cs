using UnityEngine;

public class WanderState : IState
{
    Vector2 direction = Vector2.right;
    float turnTime;
    float minTurnTime = 1.5f, maxTurnTime = 3;

    public void Enter(BaseAI owner)
    {

    }

    public void Execute(BaseAI owner)
    {
        Move(owner);
        TurnTimer(owner);
    }

    void Move(BaseAI owner)
    {
        if (direction == Vector2.right && !owner.facingRight)
            owner.Flip();
        else if (direction == Vector2.left && owner.facingRight)
            owner.Flip();
        owner.rBody.velocity = new Vector2(owner.speed, owner.rBody.velocity.y) * direction.x;
    }

    void TurnTimer(BaseAI owner)
    {
        if (turnTime > 0)
            turnTime -= Time.deltaTime;
        else
        {
            direction = owner.RandomDirection();
            turnTime = Random.Range(minTurnTime, maxTurnTime);
        }
    }

    public void Exit(BaseAI owner)
    {
        owner.rBody.velocity = Vector2.zero; // So it doesnt slide
    }
}
