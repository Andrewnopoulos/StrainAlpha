using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FMODUnity
{
    [CustomEditor(typeof(StudioBankLoader))]
    [CanEditMultipleObjects]
    class StudioBankLoaderDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            var load = serializedObject.FindProperty("LoadEvent");
            var unload = serializedObject.FindProperty("UnloadEvent");
            var tag = serializedObject.FindProperty("CollisionTag");
            var banks = serializedObject.FindProperty("Banks");
            var preload = serializedObject.FindProperty("PreloadSamples");

            EditorGUILayout.PropertyField(load, new GUIContent("Load"));
            if (load.enumValueIndex == 1)
            {
                tag.stringValue = EditorGUILayout.TagField("Collision Tag", tag.stringValue);
            }

            EditorGUILayout.PropertyField(unload, new GUIContent("Unload"));

            EditorGUILayout.PropertyField(preload, new GUIContent("Preload Sample Data"));

            EditorGUILayout.PropertyField(banks);


            Event e = Event.current;
            if (e.type == EventType.dragPerform)
            {
                if (DragAndDrop.objectReferences.Length > 0 &&
                    DragAndDrop.objectReferences[0] != null &&
                    DragAndDrop.objectReferences[0].GetType() == typeof(EditorBankRef))
                {
                    var bankList = banks.FindPropertyRelative("Banks");
                    int pos = bankList.arraySize;
                    bankList.InsertArrayElementAtIndex(pos);
                    var pathProperty = bankList.GetArrayElementAtIndex(pos).FindPropertyRelative("Name");

                    pathProperty.stringValue = ((EditorBankRef)DragAndDrop.objectReferences[0]).Name;

                    e.Use();
                }
            }
            if (e.type == EventType.DragUpdated)
            {
                if (DragAndDrop.objectReferences.Length > 0 &&
                    DragAndDrop.objectReferences[0] != null &&
                    DragAndDrop.objectReferences[0].GetType() == typeof(EditorBankRef))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    DragAndDrop.AcceptDrag();
                    e.Use();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
