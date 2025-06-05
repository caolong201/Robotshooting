using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VSX.UniversalVehicleCombat
{
    /// <summary>
    /// Adds help links to editor menu.
    /// </summary>
    public class MCK_HelpLinks : EditorWindow
    {
        [MenuItem("Mech Combat Kit/Help/Tutorial Videos")]
        public static void TutorialVideos()
        {
            Application.OpenURL("https://vimeo.com/showcase/7173766");
        }

        [MenuItem("Mech Combat Kit/Help/Documentation")]
        public static void Documentation()
        {
            Application.OpenURL("https://vsxgames.gitbook.io/universal-vehicle-combat/");
        }

        [MenuItem("Mech Combat Kit/Help/Forum")]
        public static void Forum()
        {
            Application.OpenURL("https://forum.unity.com/threads/mech-combat-kit.852241/");
        }
    }
}

