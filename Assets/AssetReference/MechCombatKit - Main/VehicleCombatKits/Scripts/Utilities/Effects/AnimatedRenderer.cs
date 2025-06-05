using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Stores a reference to a renderer and its color key, and makes it easy to modify its material properties.
    /// </summary>
    [System.Serializable]
    public class AnimatedRenderer
    {

        public Renderer renderer;
        public bool overrideMaterialColorID = false;
        public string materialColorIDOverride = "_Color";
        public bool preserveAlpha = false;
        protected float baseAlpha = 1;
        public float BaseAlpha { get { return baseAlpha; } }


        public AnimatedRenderer(Renderer renderer, string materialColorIDOverride = "")
        {
            this.renderer = renderer;
            if (materialColorIDOverride != "")
            {
                this.materialColorIDOverride = materialColorIDOverride;
                baseAlpha = renderer.material.GetColor(materialColorIDOverride).a;
                overrideMaterialColorID = true;
            }
            else
            {
                baseAlpha = renderer.material.color.a;
            }
        }


        /// <summary>
        /// Set the alpha of this animated renderer.
        /// </summary>
        /// <param name="alpha">The alpha for this renderer.</param>
        public virtual void SetAlpha(float alpha)
        {
            if (overrideMaterialColorID)
            {
                Color c = renderer.material.GetColor(materialColorIDOverride);
                c.a = alpha;
                renderer.material.SetColor(materialColorIDOverride, c);
            }
            else
            {
                Color c = renderer.material.color;
                c.a = alpha;
                renderer.material.color = c;
            }
        }


        /// <summary>
        /// Get the color of this animated renderer.
        /// </summary>
        /// <returns>The color.</returns>
        public virtual Color GetColor()
        {
            if (overrideMaterialColorID)
            {
                return renderer.material.GetColor(materialColorIDOverride);
            }
            else
            {
                return renderer.material.color;
            }
        }


        /// <summary>
        /// Set the color of this animated renderer.
        /// </summary>
        /// <param name="newColor">The new color.</param>
        public virtual void SetColor(Color newColor)
        {
            if (overrideMaterialColorID)
            {
                renderer.material.SetColor(materialColorIDOverride, newColor);
            }
            else
            {
                renderer.material.color = newColor;
            }
        }
    }
}
