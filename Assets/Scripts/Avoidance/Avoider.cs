using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AvoidanceLogic
{
    public class Avoider : MonoBehaviour, IAvoider
    {
        private List<AvoiderChecker> objectsToAvoid = new List<AvoiderChecker>();

        void Update() {
            CleanUpObjectsToAvoid();
        }

        private void CleanUpObjectsToAvoid() {
            List<int> indexOfObjectsToRemove = new List<int>();
            int index = 0;
            objectsToAvoid.ForEach((objectToAvoid) => {
                if (objectToAvoid == null) {
                    indexOfObjectsToRemove.Add(index);
                }
                index++;
            });

            indexOfObjectsToRemove.ForEach((index) => {
                objectsToAvoid.RemoveAt(index);
            });
        }

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

            foreach (var objectToAvoid in objectsToAvoid)
            {

                if (ObjectIsBehindAvoider(preferredDirection, objectToAvoid, currentPosition)) continue;
                Vector3 avoidanceDirection = GetPreferredAvoidanceDirectionFromOneObject(
                    objectToAvoid,
                    preferredDirection,
                    currentPosition
                );

                sumOfAllAvoidanceDirections += avoidanceDirection;
            }

            Vector3 avarageOfAllAvoidanceDirections = sumOfAllAvoidanceDirections / (objectsToAvoid.Count * 1.0f);
            return avarageOfAllAvoidanceDirections;
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
                float detectFactor = Math.Max(1 - ((Vector3.Distance(currentPosition, objectToAvoidPosition) - objectToAvoid.avoidRadius) / (objectToAvoid.detectRadius - objectToAvoid.avoidRadius)), 0.0f);
                float avoidFactor = Math.Max(1 - (avoidanceVector.magnitude / objectToAvoid.avoidRadius), 0.0f);
                return avoidFactor * detectFactor;
        }

        private bool ObjectIsBehindAvoider(Vector3 preferredDirection, AvoiderChecker objectToAvoid, Vector3 currentPosition) {
            Vector3 vectorToObjectToAvoid = GetVectorToObjectToAvoid(objectToAvoid, currentPosition);
            Vector3 projectedVector = Vector3.Project(vectorToObjectToAvoid, preferredDirection);
            float errorMargin = 0.1f;
            bool isBehind = preferredDirection.magnitude + projectedVector.magnitude - errorMargin > (projectedVector + preferredDirection).magnitude;
            return isBehind;
        }

        private Vector3 GetVectorToObjectToAvoid(AvoiderChecker objectToAvoid, Vector3 currentPosition) {
            return objectToAvoid.transform.position - currentPosition;
        }

        public AvoiderChecker GetClosestMovingAvoidanceObject(Vector3 currentPosition, Vector3 preferredDirection) {
            AvoiderChecker closestObject = null;
            float closestDistance = 2000.0f;
            foreach (var objectToAvoid in objectsToAvoid)
            {
                if (objectToAvoid.movingObject == false) continue;
                if (ObjectIsBehindAvoider(preferredDirection, objectToAvoid, currentPosition)) continue;

                float distance = Vector3.Distance(currentPosition, objectToAvoid.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestObject = objectToAvoid;
                }
            }

            return closestObject;
        }
    }
}
