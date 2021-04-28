using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Locks and hides the cursor to the centre of the screen
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        
        player.Rotate(Vector3.up * mouseX);
    }
}

