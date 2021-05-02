using System.Collections;
using System.Collections.Generic;
using AvoidanceLogic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Vector3 movementDirection = Vector3.forward;
    public float speed = -2.0f;

    IAvoider avoiderComponent;

    private Quaternion initialHeadRotation;

    private Vector3 headRotationTarget = Vector3.zero;

    public float headRotationSpeed = 0.01f;

    private bool isShyNPC = false;

    private Vector3 preferredLookAwayDirection;


    // Start is called before the first frame update
    void Start()
    {
        this.avoiderComponent = this.GetComponent<Avoider>();

        Animator animator = this.GetComponent<Animator>();
        initialHeadRotation = animator.GetBoneTransform(HumanBodyBones.Head).rotation;

        this.isShyNPC = Random.Range(0.0f, 1.0f) > 0.5f;
        this.preferredLookAwayDirection = new Vector3(Random.Range(-10f, 10f), Random.Range(10f, 20f), 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

        // LateUpdate is used to override the animator controller
    void LateUpdate()
    {   
        HandleHeadMovement();
    }

    void HandleHeadMovement() {
        if (this.avoiderComponent == null) return;

        AvoiderChecker closestObject = this.avoiderComponent.GetClosestAvoidanceObject(transform.position);
        bool isAvoiding = closestObject != null;

        if (isShyNPC) {
            LookAwayFromObjectToAvoid(closestObject);
        }
        else {
            LookAtObjectToAvoid(closestObject);
        }

        
    }

    void LookAtObjectToAvoid(AvoiderChecker objectToLookAt) {

        Animator animator = this.GetComponent<Animator>();
        Transform headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
        bool isAvoiding = objectToLookAt != null;

        if (isAvoiding) {
            Quaternion quaternionToAvoidObject = Quaternion.LookRotation((transform.position - objectToLookAt.transform.position).normalized);
            Vector3 eulerAnglesToAvoidObject = quaternionToAvoidObject.eulerAngles;

            // This is super weird! The head rotation seems to be different from normal Unity rotation, so instead of applying
            // the euler directly, we only apply the y angle to the x angle for the head. Do not know why it works but is somehow does.
            // headAngleTarget = Mathf.MoveTowardsAngle(headAngleTarget, eulerAnglesToAvoidObject.y, headRotationSpeed);
            UpdateHeadRotationAngle(new Vector3(-eulerAnglesToAvoidObject.y, 0, 0));
        }
        else {
            UpdateHeadRotationAngle(Vector3.zero);
        }

        headTransform.rotation = initialHeadRotation * Quaternion.Euler(headRotationTarget); 
    }

    void LookAwayFromObjectToAvoid(AvoiderChecker objectToLookAt) {
        Animator animator = this.GetComponent<Animator>();
        Transform headTransform = animator.GetBoneTransform(HumanBodyBones.Head);

        bool isAvoiding = objectToLookAt != null;
        if (isAvoiding) {
            UpdateHeadRotationAngle(preferredLookAwayDirection);
        }
        else {
            UpdateHeadRotationAngle(Vector3.zero);
        }

        headTransform.localRotation = Quaternion.Euler(headRotationTarget); 
    }

    void UpdateHeadRotationAngle(Vector3 newEulerAngle) {
        headRotationTarget = new Vector3(
            Mathf.MoveTowardsAngle(headRotationTarget.x, newEulerAngle.x, headRotationSpeed * Time.deltaTime),
            Mathf.MoveTowardsAngle(headRotationTarget.y, newEulerAngle.y, headRotationSpeed * Time.deltaTime),
            Mathf.MoveTowardsAngle(headRotationTarget.z, newEulerAngle.z, headRotationSpeed * Time.deltaTime)
        );
    }

    void HandleMovement() {
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
