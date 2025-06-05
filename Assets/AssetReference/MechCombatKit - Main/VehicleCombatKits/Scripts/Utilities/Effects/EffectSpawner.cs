using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.Utilities
{
    public class EffectSpawner : ObjectSpawner
    {
        protected override GameObject SpawnObject(GameObject objectToSpawn)
        {
            GameObject obj = base.SpawnObject(objectToSpawn);

            EffectController effectController = obj.GetComponent<EffectController>();
            if (effectController != null) effectController.SetScale(scale);

            return obj;
        }
    }

}
