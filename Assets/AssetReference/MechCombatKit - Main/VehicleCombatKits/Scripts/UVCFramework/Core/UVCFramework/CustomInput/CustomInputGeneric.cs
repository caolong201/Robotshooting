using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSX.Controls
{
    /// <summary>
    /// Create custom input using Unity Events.
    /// </summary>
    public class CustomInputGeneric : GeneralInput
    {

        [SerializeField]
        protected List<CustomInputEventItem> inputItems = new List<CustomInputEventItem>();

        
        protected override void OnInputUpdate()
        {
            // Run the input items
            for (int i = 0; i < inputItems.Count; ++i)
            {
                inputItems[i].ProcessEvents();
            }
        }
    }
}