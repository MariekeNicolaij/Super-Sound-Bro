using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plug : MonoBehaviour
{
    public Rigidbody2D rBody;
    public DistanceJoint2D joint2D;
    SoundParticleSystem sps;
    GameObject imageObject;
    BoxCollider2D triggerCollider;

    Player player;

    Vector2 startPosition;
    bool startAnimation;
    float animationTime = 0;

    float minScale, maxScale;
    float scaleMultiplyer = 0.75f;
    float closeFactor = 0.01f;


    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        triggerCollider = GetComponent<BoxCollider2D>();

        if (!joint2D)
                joint2D = (!GetComponent<DistanceJoint2D>()) ? gameObject.AddComponent<DistanceJoint2D>() : GetComponent<DistanceJoint2D>();
        joint2D.connectedBody = player.rBody;
        joint2D.enabled = false;

        imageObject = GetComponentInChildren<SpriteRenderer>().gameObject;

        startPosition = transform.position;
        maxScale = transform.localScale.x;
        minScale = maxScale * scaleMultiplyer;

        transform.localScale = new Vector2(minScale, minScale);
    }

    void Update()
    {
        VelocityCheck();
        FallCheck();
        if (startAnimation)
            AnimateToPosition();
    }

    void VelocityCheck()
    {
        //Debug.Log(rBody.velocity);
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
        if (other.tag == "Plug Animation Trigger")
            startAnimation = true;
    }

    void AnimateToPosition()
    {
        // Plays animation slowly
        animationTime += Time.deltaTime;
        // Player cannot hold it anymore
        player.Hold(false);
        // Gets closest finish plug
        GameObject finishPlug = PlugManager.instance.GetClosestFinishPlug(transform.position);
        // Get its particle system
        sps = finishPlug.GetComponentInChildren<SoundParticleSystem>();

        // Disables the trigger collider to make sure player cannot interact with it
        triggerCollider.enabled = false;
        // Disable joint because it will not be needed anymore
        joint2D.enabled = false;
        // Disable rigidbody because it will not be needed anymore and it can animate to its final position properly
        rBody.isKinematic = true;
        rBody.Sleep();
        
        // Makes the particles life time shorter aka the player cant touch them anymore
        sps.ChangeLifeTime(true);
        // Player is now unable to pick this plug up
        PlugManager.instance.RemovePlugFromList(this);

        // Gets the muffle filter and desired frequency
        AudioLowPassFilter muffleFilter = AudioManager.instance.muffleFilter;
        float muffleFrequency = AudioManager.instance.muffleFrequency;

        // Lerps audio frequency to a muffled frequency
        muffleFilter.cutoffFrequency = Mathf.Lerp(muffleFilter.cutoffFrequency, muffleFrequency, Time.deltaTime);
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

    public void ToggleWalkThroughPlug(bool canWalkThroughPlug)
    {
        //imageObject.layer = (canWalkThroughPlug) ? 9 : 0;   // 0 = Default, 9 = Plug

        if(!canWalkThroughPlug)
            StartCoroutine(EnableWalkThroughPlugDelay(0.5f));      // Delay
        else
            imageObject.layer = 9;   // 0 = Default, 9 = Plug. In physics matrix player cant collide with plug
    }

    /// <summary>
    /// Prevents the plug from hitting the player first
    /// </summary>
    /// <returns></returns>
    IEnumerator EnableWalkThroughPlugDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        imageObject.layer = 0;   // 0 = Default, 9 = Plug. In physics matrix player cant collide with plug
    }
}
