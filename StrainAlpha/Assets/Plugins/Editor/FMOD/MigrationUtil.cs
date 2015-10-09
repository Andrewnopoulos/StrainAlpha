using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;
/*
public partial class FMOD_StudioEventEmitter : MonoBehaviour
{
}

public partial class FMOD_Listener : MonoBehaviour
{
}

namespace FMODUnity
{
    [InitializeOnLoad]
    public class MigrationUtil : MonoBehaviour
    {
        [MenuItem("FMOD/Migration")]
        public static void ShowMigrationDialog()
        {
            if (EditorUtility.DisplayDialog("FMOD Studio Integration Migration", "Are you sure you wish to migrate.\nPlease backup your Unity project first.", "OK", "Cancel"))
            {
                Migrate();
            }
        }

        static string[] OldScripts = 
        {
            "Assets/Editor/FMODEventEmitterInspector.cs",
            "Assets/Editor/FMODEventInspector.cs",
            "Assets/Plugins/FMOD/Editor/FMODEditorExtension.cs",
            "Assets/Plugins/FMOD/FMOD_Listener.cs",
            "Assets/Plugins/FMOD/FMOD_StudioEventEmitter.cs",
            "Assets/Plugins/FMOD/FMOD_StudioSystem.cs",
            "Assets/Plugins/FMOD/FMODAsset.cs",
            "Assets/StreamingAssets/FMOD_bank_list.txt",
        };

        public static void Migrate()
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("FMOD Studio Integration Migration");

            Settings settings = Settings.Instance;

            var prefKey = "FMODStudioProjectPath_" + Application.dataPath;
            var prefValue = EditorPrefs.GetString(prefKey);
            if (prefValue != null)
            {
                settings.SourceBankPath = prefValue as string;
                settings.SourceBankPath += "/Build";
                settings.HasSourceProject = false;
            }

            // for each level
            EditorApplication.SaveCurrentSceneIfUserWantsTo();

            var scenes = AssetDatabase.FindAssets("*.scene");
            foreach (string scene in scenes)
            {
                if (!EditorUtility.DisplayDialog("FMOD Studio Integration Migration", String.Format("Migrate scene {0}", AssetDatabase.GUIDToAssetPath(scene)), "OK", "Cancel"))
                {
                    continue;
                }

                EditorApplication.OpenScene(AssetDatabase.GUIDToAssetPath(scene));

                var emitters = FindObjectsOfType<FMOD_StudioEventEmitter>();
                foreach (var emitter in emitters)
                {
                    GameObject parent = emitter.gameObject;
                    bool startOnAwake = emitter.startEventOnAwake;
                    string path = null;
                    if (emitter.asset != null)
                    {
                        path = emitter.asset.path;
                    }
                    else if (!String.IsNullOrEmpty(emitter.path))
                    {
                        path = emitter.path;
                    }
                    else
                    {
                        continue;
                    }

                    Undo.DestroyObjectImmediate(emitter);

                    var newEmitter = Undo.AddComponent<StudioEventEmitter>(parent);
                    newEmitter.Event = new EventRef();
                    newEmitter.Event.Path = path;
                    newEmitter.Scope = startOnAwake ? Scope.LevelStart : Scope.Manual;
                }


                var listeners = FindObjectsOfType<FMOD_Listener>();

                foreach (var listener in listeners)
                {
                    GameObject parent = listener.gameObject;

                    foreach (var plugin in listener.pluginPaths)
                    {
                        if (!settings.Plugins.Contains(plugin))
                        {
                            settings.Plugins.Add(plugin);
                        }
                    }

                    Undo.DestroyObjectImmediate(listener);
                    Undo.AddComponent<Listener>(parent);

                    // Emulate behaviour of listener loading all banks at level load
                    var loader = Undo.AddComponent<StudioBankLoader>(parent);
                    loader.Banks = new BankRefList();
                    loader.Banks.AllBanks = true;
                    loader.Scope = Scope.LevelStart;
                }

                EditorApplication.SaveCurrentSceneIfUserWantsTo();
            }

            EditorUtility.SetDirty(settings);

            foreach (var script in OldScripts)
            {
                bool success = AssetDatabase.DeleteAsset(script);
                if (success)
                {
                    UnityEngine.Debug.LogFormat("FMOD Studio: Migration removed stale script {0}", script);
                }
                else
                {
                    UnityEngine.Debug.LogWarningFormat("FMOD Studio: Migration failed to remove stale script {0}", script);
                }
            }

            AssetDatabase.DeleteAsset("Assets/FMODAssets");
            AssetDatabase.Refresh();
        }
    }
}
*/