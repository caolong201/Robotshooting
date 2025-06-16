using UnityEngine;
using UnityEditor;

public class RemoveMeshCollidersEditor : MonoBehaviour
{
    [ContextMenu("Remove Mesh Colliders from Children")]
    public void RemoveMeshColliders()
    {
        MeshCollider[] meshColliders = GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider collider in meshColliders)
        {
            DestroyImmediate(collider); // Use DestroyImmediate for editor execution
        }
        Debug.Log("Removed all MeshColliders from child objects.");
        
        BoxCollider[] bColliders = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider collider in bColliders)
        {
            DestroyImmediate(collider); // Use DestroyImmediate for editor execution
        }
        Debug.Log("Removed all MeshColliders from child objects.");
    }
}
