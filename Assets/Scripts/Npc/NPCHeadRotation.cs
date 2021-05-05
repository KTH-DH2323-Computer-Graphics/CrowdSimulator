using AvoidanceLogic;
using UnityEngine;

public class NPCHeadRotation : MonoBehaviour
{

    IAvoider avoiderComponent;

    private Vector3 headRotationTarget = Vector3.zero;

    public float headRotationSpeed = 0.01f;

    private bool isShyNPC = false;

    private Vector3 preferredLookAwayDirection;

    private Transform headTransform;
    private NPCMovement movement;

    private Quaternion currentHeadRotation;


    // Start is called before the first frame update
    void Start()
    {
        this.avoiderComponent = this.GetComponent<Avoider>();

        this.movement = this.GetComponent<NPCMovement>();

        Animator animator = this.GetComponent<Animator>();
        headTransform = animator.GetBoneTransform(HumanBodyBones.Head);

        this.isShyNPC = Random.Range(0.0f, 1.0f) > 0.5f;
        this.preferredLookAwayDirection = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0.0f);
    }


    // Update is called once per frame
    void Update()
    {
    }

        // LateUpdate is used to override the animator controller
    void LateUpdate()
    {   
        UpdateHeadRotation();
        SetHeadRotation();
    }

    void SetHeadRotation() {
        headTransform.rotation = currentHeadRotation * Quaternion.Euler(0, 0, -90.0f);
    }

    void UpdateHeadRotation() {

        if (this.avoiderComponent == null) return;
        AvoiderChecker closestObject = this.avoiderComponent.GetClosestMovingAvoidanceObject(transform.position, this.movement.preferredWalkingDirection);

        if (closestObject == null) {
            UpdateHeadRotationAngle(GetDirectionLookRotation());
            return;
        }

        if (isShyNPC) {
            LookAwayFromObjectToAvoid(closestObject);
        }
        else {
            LookAtObjectToAvoid(closestObject);
        }
    }

    Quaternion GetDirectionLookRotation() {
        return Quaternion.LookRotation(this.movement.currentWalkingDirection);
    }

    void LookAtObjectToAvoid(AvoiderChecker objectToLookAt) {
        Vector3 lookDirection = (objectToLookAt.transform.position - headTransform.position);
        var newHeadRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        UpdateHeadRotationAngle(newHeadRotation);
    }

    void LookAwayFromObjectToAvoid(AvoiderChecker objectToLookAt) {
        UpdateHeadRotationAngle(GetDirectionLookRotation() * Quaternion.Euler(preferredLookAwayDirection));
    }

    void UpdateHeadRotationAngle(Quaternion newRotation) {
        currentHeadRotation = Quaternion.Slerp(currentHeadRotation, newRotation, headRotationSpeed * Time.deltaTime);
    }

}
