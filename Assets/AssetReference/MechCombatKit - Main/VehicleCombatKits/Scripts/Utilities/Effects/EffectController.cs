using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.Utilities
{
    public class EffectController : MonoBehaviour
    {
        [Tooltip("All the lights that are part of the explosion. Their ranges will be multiplied by the scale of the transform.")]
        [SerializeField]
        protected List<Light> lights = new List<Light>();

        protected List<float> lightBaseRanges = new List<float>();


        protected virtual void Awake()
        {
            for (int i = 0; i < lights.Count; ++i)
            {
                lightBaseRanges.Add(lights[i].range);
            }

            SetScale(transform.localScale.x);
        }


        public virtual void SetScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);

            for (int i = 0; i < lights.Count; ++i)
            {
                lights[i].range = lightBaseRanges[i] * scale;
            }
        }

    }
}

