using System.Collections;
using System.Collections.Generic;
using AvoidanceLogic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 preferredWalkingDirection = Vector3.forward;
    
    [HideInInspector]
    public Vector3 currentWalkingDirection = Vector3.forward;

    private float speed = 2.0f;

    public float directionChangeSpeed = 0.2f;

    IAvoider avoiderComponent;

    void Start()
    {
        this.avoiderComponent = this.GetComponent<Avoider>();
    }

    void Update()
    {
        UpdateMovement();
        SetMovement();
    }

    void UpdateMovement() {
        Vector3 avoidanceVector = avoiderComponent.GetPreferredAvoidanceVector(
            this.gameObject.transform.position, 
            preferredWalkingDirection
        );
        Vector3 newPreferredDirection = (preferredWalkingDirection * (1 - avoidanceVector.magnitude) + avoidanceVector) * speed;
        currentWalkingDirection = Vector3.MoveTowards(currentWalkingDirection, newPreferredDirection, directionChangeSpeed * Time.deltaTime);
    }

    void SetMovement() {
        transform.rotation = Quaternion.LookRotation(currentWalkingDirection);
        transform.Translate(currentWalkingDirection * Time.deltaTime, Space.World);
    }

    public void SetWalkingData(float speed, Vector3 walkingDirection) {
        this.preferredWalkingDirection = walkingDirection;
        this.currentWalkingDirection = walkingDirection;
        this.speed = speed;
    }
}
