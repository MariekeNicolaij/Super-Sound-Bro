using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject laserObject;         // It points to cursor / right-joystick
    Image laserImage;
    Color defaultLaserColor;
    float laserDelay;
    float maxLaserDelay = 1;


    void Start()
    {
        if (!laserObject)
            laserObject = GameObject.FindGameObjectWithTag("Laser");
        laserImage = laserObject.GetComponentInChildren<Image>();
        defaultLaserColor = laserImage.color;
    }

    void Update()
    {
        LaserToCursor();
        Inputs();
    }

    void LaserToCursor()
    {
        Vector2 mouse = Input.mousePosition;
        laserObject.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg - 90);
    }

    void Inputs()
    {
        if (Input.GetButtonDown("Fire"))

            Debug.Log(CanFire());
        if (CanFire() && Input.GetButtonDown("Fire"))
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
        {
            ResetLaserColor();
            return true;
        }
    }

    void ResetLaserColor()
    {
        laserImage.color = defaultLaserColor;
    }

    void Fire()
    {
        Color newTempColor;
        laserDelay = maxLaserDelay;

        if (HitSoundObject())
        {
            newTempColor = Color.green;
            //add score depending how accurate shooting was
        }
        else
            newTempColor = Color.red;
        newTempColor.a = defaultLaserColor.a;
        laserImage.color = newTempColor;
    }

    bool HitSoundObject()
    {
        Debug.Log("This does not work");
        Collider2D laserCollider = laserImage.GetComponent<Collider2D>();

        foreach (GameObject go in AudioManager.instance.soundList)
        {
            Debug.Log(laserCollider.bounds);
            Collider2D goCollider = go.GetComponent<Collider2D>();
            Debug.Log(goCollider.bounds);
            if (laserCollider.bounds.Intersects(goCollider.bounds))
                return true;
        }
        return false;
    }

    Vector3 CursorDirection()
    {
        return Vector3.zero;
    }
}
