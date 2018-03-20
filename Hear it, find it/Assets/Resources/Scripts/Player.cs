using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Sprite laserSprite;         // It points to cursor / right-joystick
    float laserDelay;
    float maxLaserDelay = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if (Input.GetButtonDown("Fire"))
        
            Debug.Log(CanFire());
         if (CanFire()&&Input.GetButtonDown("Fire"))
            Fire();
    }

    bool CanFire()
    {
        if (laserDelay > 0)
        {
            laserDelay -= Time.deltaTime;
            return false;
        }
        else
            return true;
    }

    void Fire()
    {
        laserDelay = maxLaserDelay;
    }

    Vector3 CursorDirection()
    {
        return Vector3.zero;
    }
}
