using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Interface for a component that uses the HUD camera.
    /// </summary>
    public interface IHUDCameraUser
    {
        Camera HUDCamera { set; }
    }
}
