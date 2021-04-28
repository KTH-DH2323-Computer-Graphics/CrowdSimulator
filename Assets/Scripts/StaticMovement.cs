using System.Collections;
using System.Collections.Generic;
using AvoidanceLogic;
using UnityEngine;

public class StaticMovement : MonoBehaviour
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
        Vector3 avoidanceVector = avoiderComponent.GetPreferredAvoidanceVector(this.gameObject.transform.position, movementDirection);
        transform.Translate((movementDirection * speed + avoidanceVector) * Time.deltaTime, Space.World);
    }
}
