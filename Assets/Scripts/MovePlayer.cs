using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
    
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // To handle gravity when grounded
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        // Get horizontal and vertical input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // Player will move based on what direction they are facing
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Apply the movement
        controller.Move(move * speed * Time.deltaTime); 

        // To apply gravity to the player
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
