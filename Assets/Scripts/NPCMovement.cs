using System.Collections;
using System.Collections.Generic;
using AvoidanceLogic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Vector3 movementDirection = Vector3.forward;
    public float speed = -2.0f;
    
    IAvoider avoiderComponent;

    // Start is called before the first frame update
    void Start()
    {
        this.avoiderComponent = this.GetComponent<Avoider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 avoidanceVector = avoiderComponent.GetPreferredAvoidanceVector(
            this.gameObject.transform.position, 
            movementDirection
        );
        Vector3 finalMovement = movementDirection * speed + avoidanceVector;
        Vector3 finalDirection = finalMovement.normalized;

        transform.rotation = Quaternion.LookRotation(finalDirection);
        transform.Translate(finalDirection * speed * Time.deltaTime, Space.World);
    }
}
