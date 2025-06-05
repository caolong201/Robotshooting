using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VSX.UniversalVehicleCombat
{
    public class VehicleInterior : MonoBehaviour
    {
        [SerializeField]
        protected Vehicle vehicle;
        public Vehicle Vehicle { get { return vehicle; } }


        protected List<VehicleInteriorVolume> volumes = new List<VehicleInteriorVolume>();
        public List<VehicleInteriorVolume> Volumes { get { return volumes; } }

        public bool disableRenderers = true;

        public bool disableGameObjectOnPlayerExited = true;

        public UnityEvent onPlayerEntered;
        public UnityEvent onPlayerExited;

        protected List<Renderer> renderers = new List<Renderer>();


        protected virtual void Awake()
        {
            volumes = new List<VehicleInteriorVolume>(GetComponentsInChildren<VehicleInteriorVolume>());
            renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        }


        public virtual void CheckLayers(LayerMask layerMask)
        {
            CheckLayersRecursive(transform, layerMask);
        }

        protected virtual void CheckLayersRecursive(Transform t, LayerMask layerMask)
        {
            if (!(layerMask == (layerMask | (1 << t.gameObject.layer))))
            {
                Debug.LogWarning("Game Object found on interior of vehicle " + vehicle.name + " which does not belong to the interior layer mask. This may cause problems.");
            }
        }


        public virtual void OnPlayerEntered()
        {
            if (transform.IsChildOf(vehicle.transform)) transform.SetParent(null);
            transform.position = InteriorRenderingManager.Instance.InteriorSpawnPosition;

            gameObject.SetActive(true);

            if (disableRenderers)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = false;
                }
            }

            onPlayerEntered.Invoke();
        }


        public virtual void OnPlayerExited()
        {
            if (disableGameObjectOnPlayerExited)
            {
                gameObject.SetActive(false);
            }

            onPlayerExited.Invoke();
        }
    }
}
