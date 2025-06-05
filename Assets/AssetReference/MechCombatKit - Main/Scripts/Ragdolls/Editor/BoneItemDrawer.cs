using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace VSX.UniversalVehicleCombat
{
    [CustomPropertyDrawer(typeof(GenericRagdollWizard.BoneItem))]
    public class BoneItemDrawer : PropertyDrawer
    {
        private float itemHeight = 16;
        private float spacing = 2;
        private float border = 10;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return itemHeight * 4 + border * 2 + spacing * 3;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.Box(position, GUIContent.none, EditorStyles.helpBox);


            Rect runningPosition = position;

            runningPosition.x += border;
            runningPosition.y += border;
            runningPosition.height = itemHeight;
            runningPosition.width -= border * 2;


            EditorGUI.PropertyField(runningPosition, property.FindPropertyRelative("bone"));

            runningPosition.y += itemHeight + spacing;
            EditorGUI.PropertyField(runningPosition, property.FindPropertyRelative("boneEnd"));

            runningPosition.y += itemHeight + spacing;
            EditorGUI.PropertyField(runningPosition, property.FindPropertyRelative("addCharacterJoint"));

            runningPosition.y += itemHeight + spacing;
            EditorGUI.PropertyField(runningPosition, property.FindPropertyRelative("colliderType"));

        }
    }
}

