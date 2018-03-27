using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenSpriteAnimation : MonoBehaviour
{
    public Sprite mainSprite;      // This sprite will be used as main animation background sprite. (falling naimation)
    List<GameObject> sprites = new List<GameObject>();

    [Range(1, 25)]
    public float maxSprites = 20;
    [Range(1, 5)]
    public float maxSpriteScale = 2.5f;

    float minHorizontal = 20;
    float minVertical = 10;


    void Start()
    {
        SpawnSprites();
    }

    void SpawnSprites()
    {
        for (int i = 0; i < maxSprites; i++)
        {
            GameObject go = new GameObject("Sprite is nasty");
            go.AddComponent<SpriteRenderer>().sprite = mainSprite;
            go.AddComponent<Rigidbody2D>().drag = Random.Range(0.5f, 2f);

            Respawn(go);
            go.transform.parent = transform;


            sprites.Add(go);
        }
    }

    void Update()
    {
        RespawnCheck();
    }

    void RespawnCheck()
    {
        foreach (GameObject go in sprites)
            if (go.transform.position.y < -20)
                Respawn(go);
    }

    void Respawn(GameObject s)
    {
        s.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        s.transform.position = RandomPosition();
        s.transform.rotation = RandomRotation();
        float randomScale = RandomScale();
        s.transform.localScale = new Vector2(randomScale, randomScale);
    }

    Vector2 RandomPosition()
    {
        return new Vector2(Random.Range(-minHorizontal, minHorizontal), minVertical);
    }

    Quaternion RandomRotation()
    {
        return Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
    }

    float RandomScale()
    {
        return Random.Range(1, maxSpriteScale);
    }
}
