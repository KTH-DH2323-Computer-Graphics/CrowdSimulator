using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
       float x = Input.GetAxis("Horizontal");
       
       float z = Input.GetAxis("Vertical");
       
       Vector3 move = transform.right * x + transform.forward * z; //Player will move based on what direction they are facing
       
       controller.Move(move * speed * Time.deltaTime); 
    }
}
