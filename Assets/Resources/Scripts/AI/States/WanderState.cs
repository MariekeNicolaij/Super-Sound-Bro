using UnityEngine;

public class WanderState : IState
{
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
        if (owner.direction == Vector2.right && !owner.facingRight)
            owner.Flip();
        else if (owner.direction == Vector2.left && owner.facingRight)
            owner.Flip();
        owner.rBody.velocity = new Vector2(owner.speed, owner.rBody.velocity.y) * owner.direction.x; // Move in the right direction
    }

    void TurnTimer(BaseAI owner)
    {
        if (turnTime > 0)
            turnTime -= Time.deltaTime;
        else
        {
            owner.direction = owner.RandomDirection();
            turnTime = Random.Range(minTurnTime, maxTurnTime);
        }
    }

    public void Exit(BaseAI owner)
    {
        owner.rBody.velocity = Vector2.zero; // So it doesnt slide
    }
}
