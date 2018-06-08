using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    float dieAnimationTime = 1.5f;

    // Start
    public void Enter(BaseAI owner)
    {
        owner.dieParticleSystem.Play();
        GameObject.Destroy(owner.rBody);
        GameObject.Destroy(owner.GetComponent<BoxCollider2D>());
        owner.Invoke("StartDieAnimation", dieAnimationTime);
    }

    // Update
    public void Execute(BaseAI owner)
    {
        // Rotate enemy
        owner.transform.Rotate(Vector3.forward * Time.deltaTime * 500); // 500 = speed
        // Shrink enemy
        owner.transform.localScale = Vector3.Lerp(owner.transform.localScale, Vector3.zero , Time.deltaTime / dieAnimationTime);
    }

    public void Exit(BaseAI owner)
    {

    }
}
