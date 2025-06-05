using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VSX.UniversalVehicleCombat
{
    public class VehicleTransformAnimation : VehicleControlAnimation
    {

        Quaternion rotation = Quaternion.Euler(0, 0, 0);

        public Vector3 rotationAmount;
        public AnimationCurve rotationCurve;
        public float duration = 1;
        public float delay = 0;


        public override Quaternion GetRotation()
        {
            return rotation;
        }


        public void Action()
        {
            StartCoroutine(AnimationCoroutine());
        }

        IEnumerator AnimationCoroutine()
        {
            yield return new WaitForSeconds(delay);

            float startTime = Time.time;
            while (true)
            {
                float amount = (Time.time - startTime) / duration;
                if (amount >= 1)
                {
                    rotation = Quaternion.Euler(rotationAmount * rotationCurve.Evaluate(1));
                    break;
                }
                else
                {
                    rotation = Quaternion.Euler(rotationAmount * rotationCurve.Evaluate(amount));
                }

                yield return null;
            }
        }
    }
}

