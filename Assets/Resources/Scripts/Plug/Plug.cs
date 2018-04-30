using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    SoundParticleSystem sps;

    Vector2 startPosition;
    bool startAnimation;
    float animationTime = 0;

    float minScale, maxScale;
    float scaleMultiplyer = 0.75f;
    float closeFactor = 0.01f;


    void Start()
    {
        startPosition = transform.position;
        maxScale = transform.localScale.x;
        minScale = maxScale * scaleMultiplyer;

        transform.localScale = new Vector2(minScale, minScale);
    }

    void Update()
    {
        FallCheck();
        if (startAnimation)
            AnimateToPosition();
    }

    void FallCheck()
    {
        if (transform.position.y < -50)
        {
            transform.position = startPosition;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Plug Animation Trigger")
            startAnimation = true;
    }

    void AnimateToPosition()
    {
        Debug.Log("Test");
        // Plays animation slowly
        animationTime += Time.deltaTime;
        // Player cannot hold it anymore
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Hold(false);
        // Gets closest finish plug
        GameObject finishPlug = PlugManager.instance.GetClosestFinishPlug(transform.position);
        // Get its particle system
        sps = finishPlug.GetComponentInChildren<SoundParticleSystem>();

        // Destroy rigidbody because it will not be needed anymore
        Destroy(GetComponent<Rigidbody2D>());
        // Makes the particles life time shorter aka the player cant touch them anymore
        sps.ChangeLifeTime(true);
        // Player is now unable to pick this plug up
        PlugManager.instance.RemovePlugFromList(this);


        // Lerps to position
        transform.position = Vector2.Lerp(transform.position, finishPlug.transform.position, Time.deltaTime);
        // Lerps to rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, finishPlug.transform.rotation, Time.deltaTime);
        // Lerps to scale
        transform.localScale = Vector2.Lerp(transform.localScale, finishPlug.transform.localScale, Time.deltaTime);

        // if transformation is close
        if (Vector2.Distance(transform.position, finishPlug.transform.position) < closeFactor &&
            Vector2.Distance(transform.eulerAngles, finishPlug.transform.eulerAngles) < closeFactor)
        {
            // Set position, rotation and to finish plug its values
            transform.position = finishPlug.transform.position;
            transform.rotation = finishPlug.transform.rotation;
            transform.localScale = finishPlug.transform.localScale;

            // Done animating
            startAnimation = false;
        }
    }
}
