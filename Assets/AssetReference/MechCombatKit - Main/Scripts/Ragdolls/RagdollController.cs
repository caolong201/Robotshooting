using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Manages a ragdoll that is produced when a character is destroyed.
    /// </summary>
    public class RagdollController : MonoBehaviour
    {

        [Tooltip("The rigidbody at the root of the ragdoll.")]
        [SerializeField]
        protected Rigidbody m_Rigidbody;

        [Tooltip("The max torque applied when the ragdoll appears in the scene, to give it variation in the way it falls. This torque is applied around a random axis lying on the XZ-plane.")]
        [SerializeField]
        protected float impulseTorque = 30000;

        [Tooltip("The max force applied when the ragdoll appears in the scene, to give it variation in the way it falls and a more satisfying death effect. This force is applied along a random axis, always with a positive Y-value (upward force).")]
        [SerializeField]
        protected float impulseForce = 1000;


        protected virtual void Reset()
        {
            m_Rigidbody = GetComponentInChildren<Rigidbody>();
        }


        protected virtual void OnEnable()
        {
            // Apply torque

            Vector3 torque = Random.insideUnitSphere;
            torque.y = 0;
            torque.Normalize();
            torque *= impulseTorque;
            m_Rigidbody.AddTorque(torque);


            // Apply force

            Vector3 force = Random.insideUnitSphere;
            force.y = Mathf.Abs(force.y);
            force *= impulseForce;
            m_Rigidbody.AddForce(force);
        }
    }
}
