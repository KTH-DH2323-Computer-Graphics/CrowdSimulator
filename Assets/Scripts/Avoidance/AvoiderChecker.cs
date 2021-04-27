using System.Collections;
using System.Collections.Generic;
using AvoidanceLogic;
using UnityEngine;

public class AvoiderChecker : MonoBehaviour
{

    [Tooltip("This is the radius for when the avoider should start avoiding this object")]
    public float detectRadius = 10.0f;

    [Tooltip("This is the radius of the preferred distance that every avoider should have to this object")]
    public float avoidRadius = 3.0f;

    [HideInInspector]
    public SphereCollider detectCollider;
    
    void Start() {

    }

    void Awake ()
    {
        detectCollider = this.gameObject.AddComponent<SphereCollider>();
        detectCollider.isTrigger = true;
        detectCollider.radius = this.detectRadius;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    void OnTriggerEnter(Collider collider)
    {
        IAvoider avoider = collider.gameObject.GetComponent<IAvoider>();
        if (avoider != null) avoider.AddObjectToAvoid(this);
    }
    
    void OnTriggerExit(Collider collider)
    {
        IAvoider avoider = collider.gameObject.GetComponent<IAvoider>();
        if (avoider != null) avoider.RemoveObjectToAvoid(this);
    }
}
