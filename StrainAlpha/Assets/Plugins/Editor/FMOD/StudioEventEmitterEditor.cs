using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FMODUnity
{
    [CustomEditor(typeof(StudioEventEmitter))]
    [CanEditMultipleObjects]
    class StudioEventEmitterDrawer : Editor
    {
        bool showAdvanced;

        public override void OnInspectorGUI()
        {
            var begin = serializedObject.FindProperty("PlayEvent");
            var end = serializedObject.FindProperty("StopEvent");
            var tag = serializedObject.FindProperty("CollisionTag");
            var ev = serializedObject.FindProperty("Event");


            EditorGUILayout.PropertyField(begin, new GUIContent("Play"));
            if (begin.enumValueIndex == 1)
            {
                tag.stringValue = EditorGUILayout.TagField("Collision Tag", tag.stringValue);
            }

            EditorGUILayout.PropertyField(end, new GUIContent("Stop"));

            EditorGUILayout.PropertyField(ev, new GUIContent("Event"));

            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced Controls");
            if (showAdvanced)
            {
                var fadout = serializedObject.FindProperty("AllowFadeout");
                EditorGUILayout.PropertyField(fadout, new GUIContent("Allow Fadeout When Stopping"));
                var once = serializedObject.FindProperty("TriggerOnce");
                EditorGUILayout.PropertyField(once, new GUIContent("Trigger Once"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
