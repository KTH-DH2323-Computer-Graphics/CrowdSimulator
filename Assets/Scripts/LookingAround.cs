using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform player;

    void Start()
    {
        // Locks and hides the cursor to the centre of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Gets the mouse horizontal movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 

        // Rotate the player around the y-axis with the mouseX movement
        player.Rotate(Vector3.up * mouseX);
    }
}

