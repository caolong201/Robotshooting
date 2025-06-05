using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace VSX.UniversalVehicleCombat
{
    public class GenericRagdollWizard : ScriptableWizard
    {
        // The type of collider to add to a bone item on the ragdoll.
        public enum ColliderType
        {
            Capsule,
            Box,
            Sphere
        }


        /// <summary>
        /// Class to hold bone information for the creation of ragdoll colliders and joints.
        /// </summary>
        [System.Serializable]
        public class BoneItem
        {
            public Transform bone;
            public Transform boneEnd;
            public bool addCharacterJoint;
            public ColliderType colliderType;

            public BoneItem(Transform bone, ColliderType colliderType)
            {
                this.bone = bone;
                this.colliderType = colliderType;
            }
        }


        [Tooltip("The joint and collider settings for the bones that make up the ragdoll.")]
        public List<BoneItem> boneItems = new List<BoneItem>();


        [Tooltip("The total mass of the ragdoll.")]
        public float totalMass = 10;


        [MenuItem("Vehicle Combat Kits/Create/Ragdoll (Generic)")]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<GenericRagdollWizard>("Create Generic Ragdoll", "Create");
        }


        protected virtual void OnWizardCreate()
        {

            foreach (BoneItem item in boneItems)
            {
                Undo.AddComponent(item.bone.gameObject, typeof(Rigidbody));
                item.bone.GetComponent<Rigidbody>().mass = totalMass / boneItems.Count;

                switch (item.colliderType)
                {
                    case ColliderType.Capsule:

                        Undo.AddComponent(item.bone.gameObject, typeof(CapsuleCollider));
                        CapsuleCollider col = item.bone.gameObject.GetComponent<CapsuleCollider>();

                        if (item.boneEnd == null && item.bone.childCount > 0) item.boneEnd = item.bone.GetChild(0);

                        if (item.boneEnd != null)
                        {
                            Vector3 direction = item.boneEnd.transform.position - item.bone.transform.position;
                            col.center = item.bone.InverseTransformPoint(item.bone.transform.position + direction * 0.5f);

                            col.direction = 0;
                            float minAngle = Vector3.Angle(direction, item.bone.right);
                            minAngle = Mathf.Min(minAngle, Vector3.Angle(direction, -item.bone.right));

                            if (Vector3.Angle(direction, item.bone.up) < minAngle || Vector3.Angle(direction, -item.bone.up) < minAngle)
                            {
                                col.direction = 1;
                                minAngle = Mathf.Min(minAngle, Vector3.Angle(direction, item.bone.up));
                                minAngle = Mathf.Min(minAngle, Vector3.Angle(direction, -item.bone.up));
                            }

                            if (Vector3.Angle(direction, item.bone.forward) < minAngle || Vector3.Angle(direction, -item.bone.forward) < minAngle)
                            {
                                col.direction = 2;
                            }

                            col.height = direction.magnitude;
                            col.radius = direction.magnitude / 4;
                        }

                        break;

                    case ColliderType.Box:

                        Undo.AddComponent(item.bone.gameObject, typeof(BoxCollider));
                        break;

                    case ColliderType.Sphere:

                        Undo.AddComponent(item.bone.gameObject, typeof(SphereCollider));
                        break;
                }
            }

            foreach (BoneItem item in boneItems)
            {
                if (item.addCharacterJoint)
                {
                    Undo.AddComponent(item.bone.gameObject, typeof(CharacterJoint));
                    CharacterJoint characterJoint = item.bone.GetComponent<CharacterJoint>();
                    Rigidbody connectedBody = GetNearestParentRigidbody(item.bone);
                    characterJoint.connectedBody = connectedBody;
                }
            }
        }


        protected virtual Rigidbody GetNearestParentRigidbody(Transform t)
        {
            if (t.parent != null)
            {
                if (t.parent.GetComponent<Rigidbody>() != null) return t.parent.GetComponent<Rigidbody>();

                return GetNearestParentRigidbody(t.parent);
            }

            return null;
        }
    }
}
