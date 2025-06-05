using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Controls the effects of damage and destruction to a rigidbody character.
    /// </summary>
    public class RigidbodyCharacterDestructionController : MonoBehaviour
    {

        [Header("Settings")]

        [Tooltip("The character that is subject to damage/destruction.")]
        [SerializeField]
        protected Vehicle character;

        [Tooltip("The controller for the rigidbody character.")]
        [SerializeField]
        protected RigidbodyCharacterController characterController;

        public Animator m_Animator;

        public RigidbodyCharacterAnimations characterAnimations;

        public RigidbodyCharacterIK characterIK;

        public GimballedVehicleController gimballedVehicleController;

        [System.Serializable]
        public class DamageableMovementModifier
        {
            public Damageable damageable;
            public float movementModifier = 0.5f;
        }

        [Header("Damage")]

        [Tooltip("A list of damageables and the reduction in movement that their destruction causes.")]
        [SerializeField]
        protected List<DamageableMovementModifier> damageableMovementModifiers = new List<DamageableMovementModifier>();

        protected float currentMovementModifier = 1;

        [Header("Death")]

        public List<Collider> ragdollColliders = new List<Collider>();
        public List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
        public List<CharacterJoint> ragdollCharacterJoints = new List<CharacterJoint>();

        [Tooltip("The max torque applied when the ragdoll appears in the scene, to give it variation in the way it falls. This torque is applied around a random axis lying on the XZ-plane.")]
        [SerializeField]
        protected float deathImpulseTorque = 30000;

        [Tooltip("The max force applied when the ragdoll appears in the scene, to give it variation in the way it falls and a more satisfying death effect. This force is applied along a random axis, always with a positive Y-value (upward force).")]
        [SerializeField]
        protected float deathImpulseForce = 1000;

        [Tooltip("Whether to eject the modules from the character when it dies.")]
        [SerializeField]
        protected bool ejectModulesOnDeath = true;

        [Tooltip("The strength of the random torque applied to the modules when they are ejected.")]
        [SerializeField]
        protected float moduleEjectionTorque = 50;

        [Tooltip("The strength of the random force (applied along the direction from the character's root transform to the module position) applied to the modules when they are ejected.")]
        [SerializeField]
        protected float moduleEjectionForce = 250;


        protected virtual void Awake()
        {
            foreach (DamageableMovementModifier damageableMovementModifier in damageableMovementModifiers)
            {
                damageableMovementModifier.damageable.onDestroyed.AddListener(OnDamageableMovementModifierDestroyed);
            }

            character.onDestroyed.AddListener(OnDestroyed);
            character.onRestored.AddListener(OnRestored);
        }


        protected virtual void Start()
        {
            DisableRagdoll();
        }


        protected virtual void Reset()
        {

            character = GetComponent<Vehicle>();
            characterController = GetComponent<RigidbodyCharacterController>();
            m_Animator = GetComponentInChildren<Animator>();
            characterAnimations = GetComponent<RigidbodyCharacterAnimations>();
            characterIK = GetComponent<RigidbodyCharacterIK>();
            gimballedVehicleController = GetComponent<GimballedVehicleController>();


            ragdollCharacterJoints = new List<CharacterJoint>(GetComponentsInChildren<CharacterJoint>());
            foreach(CharacterJoint characterJoint in ragdollCharacterJoints)
            {
                if (characterJoint.GetComponent<Rigidbody>() != null) ragdollRigidbodies.Add(characterJoint.GetComponent<Rigidbody>());
                if (characterJoint.GetComponent<Collider>() != null) ragdollColliders.AddRange(characterJoint.GetComponents<Collider>());
            }
        }


        // Called when a damageable is destroyed which has an effect on movement.
        protected virtual void OnDamageableMovementModifierDestroyed()
        {
            currentMovementModifier = 1;

            for (int i = 0; i < damageableMovementModifiers.Count; ++i)
            {
                if (damageableMovementModifiers[i].damageable.Destroyed)
                {
                    currentMovementModifier -= damageableMovementModifiers[i].movementModifier;
                }
            }

            characterController.SetMovementModifier(currentMovementModifier);

        }


        public virtual void EnableRagdoll()
        {
            m_Animator.enabled = false;
            characterController.GetComponent<CapsuleCollider>().enabled = false;

            characterController.enabled = false;
            if (characterAnimations != null) characterAnimations.enabled = false;
            if (characterIK != null) characterIK.enabled = false;
            if (gimballedVehicleController != null) gimballedVehicleController.enabled = false;


            foreach (CharacterJoint joint in ragdollCharacterJoints)
            {
                //joint.enableCollision = true;
            }

            foreach (Collider col in ragdollColliders)
            {
                col.enabled = true;
            }


            foreach (Rigidbody rBody in ragdollRigidbodies)
            {
                rBody.isKinematic = false;
                //rBody.detectCollisions = true;
            }

            

        }


        protected virtual void DisableRagdoll()
        {
            m_Animator.enabled = true;
            characterController.GetComponent<CapsuleCollider>().enabled = true;

            characterController.enabled = true;
            if (characterAnimations != null) characterAnimations.enabled = true;
            if (characterIK != null) characterIK.enabled = true;
            if (gimballedVehicleController != null) gimballedVehicleController.enabled = true;



            foreach (CharacterJoint joint in ragdollCharacterJoints)
            {
                //joint.enableCollision = false;
            }

            foreach (Collider col in ragdollColliders)
            {
                col.enabled = false;
            }

            foreach (Rigidbody rBody in ragdollRigidbodies)
            {
                rBody.isKinematic = true;
                //rBody.detectCollisions = false;
            }

            
        }


        // Called when the character is destroyed.
        protected virtual void OnDestroyed()
        {

            EnableRagdoll();

            foreach (ModuleMount moduleMount in character.ModuleMounts)
            {
                for (int i = 0; i < moduleMount.Modules.Count; ++i)
                {
                    Module module = moduleMount.Modules[i];
                    moduleMount.RemoveModule(module);

                    module.gameObject.SetActive(true);

                    if (module.GetComponent<Rigidbody>() != null)
                    {
                        module.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * moduleEjectionTorque);
                        module.GetComponent<Rigidbody>().AddForce((module.transform.position - character.transform.position).normalized * moduleEjectionForce);
                    }

                    i--;
                }
            }

            // Apply torque

            Vector3 torque = Random.insideUnitSphere;
            torque.y = 0;
            torque.Normalize();
            torque *= deathImpulseTorque;
            characterController.Rigidbody.AddTorque(torque);


            // Apply force

            Vector3 force = Random.insideUnitSphere;
            force.y = Mathf.Abs(force.y);
            force *= deathImpulseForce;
            characterController.Rigidbody.AddForce(force);
        }


        // Called when the character is restored
        protected virtual void OnRestored() 
        {
            DisableRagdoll();
        }
    }
}

