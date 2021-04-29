using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvoidanceLogic
{
    public class Avoider : MonoBehaviour, IAvoider
    {

        public float maxAvoidSpeed = 10.0f;
        public float avoidChangeSpeed = 10.0f;

        private Vector3 currentAvoidVector = Vector3.zero;

        private List<AvoiderChecker> objectsToAvoid = new List<AvoiderChecker>();

        /**
            <summary>
                This adds an object that should be avoided.
            </summary>
        */
        public void AddObjectToAvoid(AvoiderChecker newObjectToAvoid)
        {
            bool newObjectAlreadyAdded = objectsToAvoid.Contains(newObjectToAvoid);
            if (newObjectAlreadyAdded) return;

            objectsToAvoid.Add(newObjectToAvoid);
        }

        /**
            <summary>
                This removes an object that should no longer be avoided.
            </summary>
        */
        public void RemoveObjectToAvoid(AvoiderChecker objectToRemove)
        {
            bool objectExist = objectsToAvoid.Contains(objectToRemove);
            if (!objectExist) return;
            objectsToAvoid.Remove(objectToRemove);
        }

        public Vector3 GetPreferredAvoidanceVector(Vector3 currentPosition, Vector3 preferredDirection)
        {
            if (objectsToAvoid.Count == 0) return Vector3.zero;

            Vector3 sumOfAllAvoidanceDirections = Vector3.zero;
            objectsToAvoid.ForEach((objectToAvoid) => {
                Vector3 avoidanceDirection = GetPreferredAvoidanceDirectionFromOneObject(
                    objectToAvoid,
                    preferredDirection,
                    currentPosition
                );
                sumOfAllAvoidanceDirections += avoidanceDirection;
            });
            Vector3 avarage = sumOfAllAvoidanceDirections / (objectsToAvoid.Count * 1.0f);

            return getAndUpdateAvoidVector(avarage);
        }

        private Vector2 getAndUpdateAvoidVector(Vector3 targetAvoidVector) {
            Vector3 difference = (targetAvoidVector - currentAvoidVector);
            float currentDistance = difference.magnitude;
            currentAvoidVector = Vector3.MoveTowards(currentAvoidVector,targetAvoidVector, avoidChangeSpeed * Time.deltaTime);
            return currentAvoidVector;
        }

        private Vector3 GetPreferredAvoidanceDirectionFromOneObject(
            AvoiderChecker objectToAvoid,
            Vector3 preferredDirection,
            Vector3 currentPosition)
        {
            // P - The object to avoid
            // D - Direction of line (unit length)
            // A - The current position of the moving object
            // X - base of the perpendicular line
            //
            //     P
            //    /|
            //   / |
            //  /  v
            // A---X----->D

            Ray ray = new Ray(currentPosition, preferredDirection.normalized);
            Vector3 p = objectToAvoid.gameObject.transform.position;
            Vector3 a = currentPosition;
            Vector3 d = ray.direction;
            Vector3 x = a + (Vector3.Dot((p - a), d)) * d;

            Vector3 avoidanceVector = (x - p);

            ///It is important that we do not want the y-axis
            avoidanceVector.y = 0;
            return avoidanceVector.normalized * GetAvoidanceVelocity(objectToAvoid, currentPosition, avoidanceVector);
        }

        private float GetAvoidanceVelocity(
            AvoiderChecker objectToAvoid,
            Vector3 currentPosition,
            Vector3 avoidanceVector
        ) {
                currentPosition.y = 0;
                Vector3 objectToAvoidPosition = objectToAvoid.gameObject.transform.position;
                objectToAvoidPosition.y = 0;

                float avoiderRadius = objectToAvoid.detectRadius;
                float detectFactor = 1 - (Vector3.Distance(currentPosition, objectToAvoidPosition) / objectToAvoid.detectRadius);
                float avoidFactor = 1 - (avoidanceVector.magnitude / objectToAvoid.avoidRadius);
                return Math.Max(detectFactor * avoidFactor * maxAvoidSpeed, 0);
        }
    }
}
