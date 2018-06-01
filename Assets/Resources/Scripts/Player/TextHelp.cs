using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHelp : MonoBehaviour
{
    public TextMesh interactionText;
    Player player;

    bool followPlayer = true;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        interactionText = GetComponent<TextMesh>();
    }

    void Update()
    {
        if (followPlayer)
            FollowPlayer();
        else
            FollowAimLineEnd();
    }

    /// <summary>
    /// Follows the player without inhereting its rotation
    /// </summary>
    void FollowPlayer()
    {
        Vector2 temp = player.transform.position;
        temp.y += 2;                                         // Offset, text will appear above players head
        interactionText.transform.position = temp;
    }

    /// <summary>
    /// When holding text should follow aim line
    /// </summary>
    void FollowAimLineEnd()
    {
        Vector2 temp = player.aimPosWorld;
        temp.y += 2;                                         // Offset, text will appear above players head
        interactionText.transform.position = temp;
    }

    /// <summary>
    /// Either shows or hides text
    /// </summary>
    /// <param name="show"></param>
    public void ToggleHoldButtonText(bool show)
    {
        followPlayer = true;
        if (Input.GetJoystickNames().Length == 0)       // Listens to mouse input
            interactionText.text = (show) ? "E" : "";
        else                                            // Controller input
            interactionText.text = (show) ? "B" : "";
    }

    /// <summary>
    /// Either shows or hides text
    /// </summary>
    /// <param name="show"></param>
    public void ToggleThrowButtonText(bool show)
    {
        followPlayer = false;
        if (Input.GetJoystickNames().Length == 0)       // Listens to mouse input
            interactionText.text = (show) ? "LMB" : "";
        else                                            // Controller input
            interactionText.text = (show) ? "X" : "";
    }
}