using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FMODUnity
{
    [CustomPropertyDrawer(typeof(BankRef))]
    class BankRefDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Texture browseIcon = EditorGUIUtility.Load("FMOD/SearchIconBlack.png") as Texture;

            SerializedProperty pathProperty = property.FindPropertyRelative("Name");

            EditorGUI.BeginProperty(position, label, property);

            Event e = Event.current;
            if (e.type == EventType.dragPerform && position.Contains(e.mousePosition))
            {
                if (DragAndDrop.objectReferences.Length > 0 &&
                    DragAndDrop.objectReferences[0] != null &&
                    DragAndDrop.objectReferences[0].GetType() == typeof(EditorBankRef))
                {
                    pathProperty.stringValue = ((EditorBankRef)DragAndDrop.objectReferences[0]).Name;

                    e.Use();
                }
            }
            if (e.type == EventType.DragUpdated && position.Contains(e.mousePosition))
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

            float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var buttonStyle = GUI.skin.button;
            buttonStyle.padding.top = buttonStyle.padding.bottom = 1;

            Rect searchRect = new Rect(position.x + position.width - browseIcon.width - 15, position.y, browseIcon.width + 10, baseHeight);
            Rect pathRect = new Rect(position.x, position.y, searchRect.x - position.x - 5, baseHeight);

            EditorGUI.PropertyField(pathRect, pathProperty, GUIContent.none);
            if (GUI.Button(searchRect, new GUIContent(browseIcon, "Select FMOD Bank"), buttonStyle))
            {
                var browser = EventBrowser.CreateInstance<EventBrowser>();
                browser.titleContent = new GUIContent("Select FMOD Bank");
                browser.SelectBank(property);
                browser.ShowUtility();
            }

            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(BankRefList))]
    class BankRefListDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            EditorGUI.BeginProperty(position, label, property);

            float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y;
            
            Rect rect = new Rect(position.x, position.y, position.width, baseHeight);

            SerializedProperty allProperty = property.FindPropertyRelative("AllBanks");
            SerializedProperty banksProperty = property.FindPropertyRelative("Banks");
            EditorGUI.PropertyField(rect, allProperty, new GUIContent("All Banks"));

            if (!allProperty.boolValue)
            {
                GUIContent addButtonContent = new GUIContent("Add Bank");
                float addButtonWidth = GUI.skin.textField.CalcSize(addButtonContent).x + 20;
                Rect buttonRect = new Rect(position.x, position.y + baseHeight + 2, addButtonWidth, baseHeight);
                if (GUI.Button(buttonRect, addButtonContent))
                {
                    banksProperty.InsertArrayElementAtIndex(banksProperty.arraySize);
                    SerializedProperty newBank = banksProperty.GetArrayElementAtIndex(banksProperty.arraySize-1);
                    newBank.FindPropertyRelative("Name").stringValue = "";

                    var browser = EventBrowser.CreateInstance<EventBrowser>();
                    browser.titleContent = new GUIContent("Select FMOD Bank");
                    browser.SelectBank(newBank);
                    browser.ShowUtility();
                }

                Texture deleteTexture = EditorGUIUtility.Load("FMOD/Delete.png") as Texture;
                GUIContent deleteContent = new GUIContent(deleteTexture, "Delete Bank");
                float deleteWidth = deleteTexture.width + 5;
                Rect deleteRect = new Rect(position.x + position.width - deleteWidth - 10, position.y + baseHeight * 2 + 4, deleteWidth, baseHeight);
                Rect bankRect = new Rect(position.x, position.y + baseHeight * 2 + 4, position.width - deleteWidth - 10, baseHeight);

                for (int i = 0; i < banksProperty.arraySize; i++)
                {
                    EditorGUI.PropertyField(bankRect, banksProperty.GetArrayElementAtIndex(i), GUIContent.none);

                    if (GUI.Button(deleteRect, deleteContent))
                    {
                        banksProperty.DeleteArrayElementAtIndex(i);
                    }
                    bankRect.y += baseHeight + 2;
                    deleteRect.y += baseHeight + 2;
                }
            }
            else
            {
                banksProperty.ClearArray();
            }

            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float baseHeight = GUI.skin.textField.CalcSize(new GUIContent()).y+2;

            SerializedProperty allProperty = property.FindPropertyRelative("AllBanks");
            bool allBanks = allProperty.boolValue;
            if (allBanks)
            {
                return baseHeight;
            }
            else
            {
                SerializedProperty banksProperty = property.FindPropertyRelative("Banks");
                return baseHeight * (banksProperty.arraySize + 2);
            }
        }
    }
}
