using UnityEngine;

namespace AvoidanceLogic
{
    public interface IAvoider
    {
        void AddObjectToAvoid(AvoiderChecker newObjectToAvoid);
        void RemoveObjectToAvoid(AvoiderChecker newObjectToAvoid);

        /// <summary>
        ///     <para>This returns a vector which is the preferred movement to avoid all the object
        ///     that this object has detected.</para>
        ///
        ///     <para>The returned vector is often used in conjunction with the current moving vector
        ///     that this object has.</para>
        /// </summary>
        Vector3 GetPreferredAvoidanceVector(Vector3 currentPosition, Vector3 preferredDirection);

        AvoiderChecker GetClosestAvoidanceObject(Vector3 currentPosition);
    }
}
