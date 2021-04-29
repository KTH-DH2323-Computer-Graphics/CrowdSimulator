using System.Collections;
using System.Collections.Generic;
using AvoidanceLogic;
using UnityEngine;

public class StaticMovement : MonoBehaviour
{

    public Vector3 movementDirection = Vector3.forward;
    public float speed = -2.0f;

    bool isAvoiding;
    float headTilt;

    IAvoider avoiderComponent;

    // Start is called before the first frame update
    void Start()
    {
        this.avoiderComponent = this.GetComponent<Avoider>();

        // Randomize head tilt on avoidance
        this.headTilt = Random.Range(0, 3f) * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 avoidanceVector = avoiderComponent.GetPreferredAvoidanceVector(this.gameObject.transform.position, movementDirection);
        isAvoiding = avoidanceVector != Vector3.zero;
        transform.Translate((movementDirection * speed + avoidanceVector) * Time.deltaTime, Space.World);
    }

    // LateUpdate is used to override the animator controller
    void LateUpdate()
    {   
        if(isAvoiding) {
            Animator animator = this.GetComponent<Animator>();
            Transform headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
            Quaternion eulerRotation = Quaternion.Euler(0,headTilt,0);
            headTransform.rotation = headTransform.rotation * eulerRotation; 
        }

    }
}