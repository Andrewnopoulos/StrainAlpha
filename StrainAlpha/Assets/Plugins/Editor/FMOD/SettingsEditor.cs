using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;

namespace FMODUnity
{
    [CustomEditor(typeof(Settings))]
    public class SettingsEditor : Editor
    {
        string[] ToggleParent = new string[] { "Disabled", "Enabled" };

        string[] FrequencyDisplay = new string[] { "Platform Default", "22050", "24000", "32000", "44100", "48000"};
        int[] FrequencyValues = new int[] { 0, 22050, 24000, 32000, 44100, 48000 };

        string[] SpeakerModeDisplay = new string[] { "Auto Detect", "Stereo", "Quad", "5.1", "7.1" };
        int[] SpeakerModeValues = new int[] { (int)FMOD.SPEAKERMODE.DEFAULT, (int)FMOD.SPEAKERMODE.STEREO, (int)FMOD.SPEAKERMODE.QUAD, (int)FMOD.SPEAKERMODE._5POINT1, (int)FMOD.SPEAKERMODE._7POINT1};

        bool[] foldoutState = new bool[(int)FMODPlatform.Count];

        bool hasBankSourceChanged = false;

        string PlatformLabel(FMODPlatform platform)
        {
            switch(platform)
            {
                case FMODPlatform.Linux:
                    return "Linux";
                case FMODPlatform.Desktop:
                    return "Desktop";
                case FMODPlatform.Console:
                    return "Console";
                case FMODPlatform.iOS:
                    return "iOS";
                case FMODPlatform.Mac:
                    return "OSX";
                case FMODPlatform.Mobile:
                    return "Mobile";
                case FMODPlatform.PS4:
                    return "PS4";
                case FMODPlatform.Windows:
                    return "Windows";
                case FMODPlatform.WindowsPhone:
                    return "Windows Phone";
                case FMODPlatform.XboxOne:
                    return "XBox One";
                case FMODPlatform.WiiU:
                    return "Wii U";
                case FMODPlatform.PSVita:
                    return "PS Vita";
                case FMODPlatform.Android:
                    return "Android";
                case FMODPlatform.MobileHigh:
                    return "High-End Mobile";
                case FMODPlatform.MobileLow:
                    return "Low-End Mobile";
            }
            return "Unknown";
        }

        void DisplayParentBool(string label, List<PlatformBoolSetting> settings, FMODPlatform platform)
        {
            bool current = Settings.GetSetting(settings, platform, false);
            int next = EditorGUILayout.Popup(label, current ? 1 : 0, ToggleParent);
            Settings.SetSetting(settings, platform, next == 1);
        }

        void DisplayChildBool(string label, List<PlatformBoolSetting> settings, FMODPlatform platform)
        {
            bool overriden = Settings.HasSetting(settings, platform);
            bool current = Settings.GetSetting(settings, platform, false);

            string[] toggleChild = new string[ToggleParent.Length + 1];
            Array.Copy(ToggleParent, 0, toggleChild, 1, ToggleParent.Length);
            toggleChild[0] = String.Format("Inherit ({0})", ToggleParent[current ? 1 : 0]);

            int next = EditorGUILayout.Popup(label, overriden ? (current ? 2 : 1) : 0, toggleChild);
            if (next == 0)
            {
                if (overriden)
                {
                    Settings.RemoveSetting(settings, platform);
                }
            }
            else
            {
                Settings.SetSetting(settings, platform, next == 2);
            }
        }

        void DisplayParentInt(string label, List<PlatformIntSetting> settings, FMODPlatform platform, int min, int max)
        {
            int current = Settings.GetSetting(settings, platform, 0);
            int next = EditorGUILayout.IntSlider(label, current, min, max);
            Settings.SetSetting(settings, platform, next);
        }

        void DisplayChildInt(string label, List<PlatformIntSetting> settings, FMODPlatform platform, int min, int max)
        {
            bool overriden = Settings.HasSetting(settings, platform);
            int current = Settings.GetSetting(settings, platform, 0);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            overriden = !GUILayout.Toggle(!overriden, "Inherit");
            int next = EditorGUILayout.IntSlider(current, min, max);
            EditorGUILayout.EndHorizontal();

            if (overriden)
            {
                Settings.SetSetting(settings, platform, next);
            }
            else
            {
                Settings.RemoveSetting(settings, platform);
            }
        }

        void DisplayParentFreq(string label, List<PlatformIntSetting> settings, FMODPlatform platform)
        {
            int current = Settings.GetSetting(settings, platform, 0);
            int index = Array.IndexOf(FrequencyValues, current);
            int next = EditorGUILayout.Popup(label, index, FrequencyDisplay);
            Settings.SetSetting(settings, platform, FrequencyValues[next]);
        }

        void DisplayChildFreq(string label, List<PlatformIntSetting> settings, FMODPlatform platform)
        {
            bool overriden = Settings.HasSetting(settings, platform);
            int current = Settings.GetSetting(settings, platform, 0);
            int index = Array.IndexOf(FrequencyValues, current);
            
            string[] valuesChild = new string[FrequencyDisplay.Length + 1];
            Array.Copy(FrequencyDisplay, 0, valuesChild, 1, FrequencyDisplay.Length);
            valuesChild[0] = String.Format("Inherit ({0})", FrequencyDisplay[index]);

            int next = EditorGUILayout.Popup(label, overriden ? index + 1 : 0, valuesChild);
            if (next == 0)
            {
                Settings.RemoveSetting(settings, platform);
            }
            else
            {
                Settings.SetSetting(settings, platform, FrequencyValues[next-1]);
            }
        }

        void DisplayParentSpeakerMode(string label, List<PlatformIntSetting> settings, FMODPlatform platform)
        {
            int current = Settings.GetSetting(settings, platform, 0);
            int index = Array.IndexOf(SpeakerModeValues, current);
            int next = EditorGUILayout.Popup(label, index, SpeakerModeDisplay);
            Settings.SetSetting(settings, platform, SpeakerModeValues[next]);
        }

        void DisplayChildSpeakerMode(string label, List<PlatformIntSetting> settings, FMODPlatform platform)
        {
            bool overriden = Settings.HasSetting(settings, platform);
            int current = Settings.GetSetting(settings, platform, 0);
            int index = Array.IndexOf(SpeakerModeValues, current);

            string[] valuesChild = new string[SpeakerModeDisplay.Length + 1];
            Array.Copy(SpeakerModeDisplay, 0, valuesChild, 1, SpeakerModeDisplay.Length);
            valuesChild[0] = String.Format("Inherit ({0})", SpeakerModeDisplay[index]);

            int next = EditorGUILayout.Popup(label, overriden ? index + 1 : 0, valuesChild);
            if (next == 0)
            {
                Settings.RemoveSetting(settings, platform);
            }
            else
            {
                Settings.SetSetting(settings, platform, SpeakerModeValues[next - 1]);
            }
        }

        void DisplayParentBuildDirectory(string label, List<PlatformStringSetting> settings, FMODPlatform platform)
        {
            string[] buildDirectories = EditorUtils.GetBankPlatforms();

            String current = Settings.GetSetting(settings, platform, "Desktop");
            int index = Array.IndexOf(buildDirectories, current);
            if (index < 0) index = 0;

            int next = EditorGUILayout.Popup(label, index, buildDirectories);
            Settings.SetSetting(settings, platform, buildDirectories[next]);
        }

        void DisplayChildBuildDirectories(string label, List<PlatformStringSetting> settings, FMODPlatform platform)
        {
            string[] buildDirectories = EditorUtils.GetBankPlatforms();

            bool overriden = Settings.HasSetting(settings, platform);
            string current = Settings.GetSetting(settings, platform, "Desktop");
            int index = Array.IndexOf(buildDirectories, current);
            if (index < 0) index = 0;

            string[] valuesChild = new string[buildDirectories.Length + 1];
            Array.Copy(buildDirectories, 0, valuesChild, 1, buildDirectories.Length);
            valuesChild[0] = String.Format("Inherit ({0})", buildDirectories[index]);

            int next = EditorGUILayout.Popup(label, overriden ? index + 1 : 0, valuesChild);
            if (next == 0)
            {
                Settings.RemoveSetting(settings, platform);
            }
            else
            {
                Settings.SetSetting(settings, platform, buildDirectories[next - 1]);
            }
        }

        void DisplayPlatform(FMODPlatform platform, FMODPlatform[] children = null)
        {
            Settings settings = target as Settings;

            var label = new global::System.Text.StringBuilder();
            label.AppendFormat("<b>{0}</b>", (PlatformLabel(platform)));
            if (children != null)
            {
                label.Append(" (");
                foreach (var child in children)
                {
                    label.Append(PlatformLabel(child));
                    label.Append(", ");
                }
                label.Remove(label.Length - 2, 2);
                label.Append(")");
            }
            
            GUIStyle style = new GUIStyle(GUI.skin.FindStyle("Foldout"));
            style.richText = true;
            
            foldoutState[(int)platform] = EditorGUILayout.Foldout(foldoutState[(int)platform], new GUIContent(label.ToString()), style);
            if (foldoutState[(int)platform])
            {
                EditorGUI.indentLevel++;
                DisplayChildBool("Live Update", settings.LiveUpdateSettings, platform);
                if (settings.IsLiveUpdateEnabled(platform))
                {
                    GUIStyle style2 = new GUIStyle(GUI.skin.label);
                    style2.richText = true;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel(" ");
                    #if UNITY_5_0 || UNITY_5_1
                    GUILayout.Label("Unity 5.0 or 5.1 detected: Live update will listen on port <b>9265</b>", style2);
                    #else
                    GUILayout.Label("Live update will listen on port <b>9264</b>", style2);
                    #endif
                    EditorGUILayout.EndHorizontal();
                }
                DisplayChildSpeakerMode("Speaker Mode", settings.SpeakerModeSettings, platform);
                DisplayChildFreq("Sample Rate", settings.SampleRateSettings, platform);
                if (settings.HasPlatforms && AllowBankChange(platform))
                {
                    bool prevChanged = GUI.changed;
                    DisplayChildBuildDirectories("Bank Platform", settings.BankDirectorySettings, platform);
                    hasBankSourceChanged |= !prevChanged && GUI.changed;
                }

                DisplayChildInt("Virtual Channel Count", settings.VirtualChannelSettings, platform, 0, 2048);
                DisplayChildInt("Real Channel Count", settings.RealChannelSettings, platform, 0, 2048);

                if (children != null)
                {
                    foreach (var child in children)
                    {
                        DisplayPlatform(child);
                    }
                }

                EditorGUI.indentLevel--;
            }
        }

        private bool AllowBankChange(FMODPlatform platform)
        {
            // Can't do these settings on pseudo-platforms
            if (platform == FMODPlatform.MobileLow || platform == FMODPlatform.MobileHigh)
            {
                return false;
            }

            return true;
        }
        
        public override void OnInspectorGUI()
        {
            Settings settings = target as Settings;
            
            EditorUtility.SetDirty(settings);

            hasBankSourceChanged = false;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("<b>Source</b>", GUI.skin.textField, style);
            int selection = settings.HasSourceProject ? 0 : 1;
            selection = GUILayout.SelectionGrid(selection, new GUIContent[2] { new GUIContent("Project"), new GUIContent("Bank Folder") }, 2, GUILayout.ExpandWidth(false));
            settings.HasSourceProject = selection == 0;
            EditorGUILayout.EndHorizontal();

            if (selection == 0)
            {
                EditorGUILayout.BeginHorizontal();
                string oldPath = settings.SourceProjectPath;
                EditorGUILayout.PrefixLabel("Studio Project Path", GUI.skin.textField, style);
                settings.SourceProjectPath = EditorGUILayout.TextField(GUIContent.none, settings.SourceProjectPath);
                if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                {
                    GUI.FocusControl(null);
                    string path = EditorUtility.OpenFilePanel("Locate Studio Project", oldPath, "fspro");
                    if (!String.IsNullOrEmpty(path))
                    {
                        settings.SourceProjectPath = path;
                        this.Repaint();
                    }
                }
                EditorGUILayout.EndHorizontal();

                // Cache in settings for runtime access in play-in-editor mode
                settings.SourceBankPath = EditorUtils.GetBankDirectory();

                settings.HasPlatforms = true;

                // First time project path is set or changes, copy to streaming assets
                if (settings.SourceProjectPath != oldPath)
                {
                    hasBankSourceChanged = true;
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                string oldPath = settings.SourceBankPath;
                EditorGUILayout.PrefixLabel("Banks Folder Path", GUI.skin.textField, style);
                settings.SourceBankPath = EditorGUILayout.TextField(GUIContent.none, settings.SourceBankPath);
                if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                {
                    GUI.FocusControl(null);
                    var path = EditorUtility.OpenFolderPanel("Locate Studio Banks", oldPath, null);
                    if (!String.IsNullOrEmpty(path))
                    {
                        settings.SourceBankPath = path;
                    }
                }
                EditorGUILayout.EndHorizontal();

                settings.HasPlatforms = EditorUtils.GetBankPlatforms().Length > 0;

                // First time project path is set or changes, copy to streaming assets
                if (settings.SourceBankPath != oldPath)
                {
                    hasBankSourceChanged = true;
                }
            }

            
            if ((settings.HasSourceProject && !File.Exists(settings.SourceProjectPath)) || !Directory.Exists(settings.SourceBankPath))
            {
                return;
            }

            // ----- PIE ----------------------------------------------
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("<b>Play In Editor Settings</b>", style);
            EditorGUI.indentLevel++;
            DisplayParentBool("Live Update", settings.LiveUpdateSettings, FMODPlatform.PlayInEditor);
            if (settings.IsLiveUpdateEnabled(FMODPlatform.PlayInEditor))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(" ");
                #if UNITY_5_0 || UNITY_5_1
                GUILayout.Label("Unity 5.0 or 5.1 detected: Live update will listen on port <b>9265</b>", style);
#else
                GUILayout.Label("Live update will listen on port <b>9264</b>", style);
                #endif
                EditorGUILayout.EndHorizontal();
            }
            DisplayParentSpeakerMode("Speaker Mode", settings.SpeakerModeSettings, FMODPlatform.PlayInEditor);
            if (settings.HasPlatforms)
            {
                DisplayParentBuildDirectory("Bank Platform", settings.BankDirectorySettings, FMODPlatform.PlayInEditor);
            }
            EditorGUI.indentLevel--;

            // ----- Default ----------------------------------------------
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("<b>Default Settings</b>", style);
            EditorGUI.indentLevel++;
            DisplayParentBool("Live Update", settings.LiveUpdateSettings, FMODPlatform.Default);
            if (settings.IsLiveUpdateEnabled(FMODPlatform.Default))
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(" ");
                #if UNITY_5_0 || UNITY_5_1
                GUILayout.Label("Unity 5.0 or 5.1 detected: Live update will listen on port <b>9265</b>", style);
#else
                GUILayout.Label("Live update will listen on port <b>9264</b>", style);
                #endif
                EditorGUILayout.EndHorizontal();
            }
            DisplayParentSpeakerMode("Speaker Mode", settings.SpeakerModeSettings, FMODPlatform.Default);
            DisplayParentFreq("Sample Rate", settings.SampleRateSettings, FMODPlatform.Default);
            if (settings.HasPlatforms)
            {
                bool prevChanged = GUI.changed;
                DisplayParentBuildDirectory("Bank Platform", settings.BankDirectorySettings, FMODPlatform.Default);
                hasBankSourceChanged |= !prevChanged && GUI.changed;
            }
            DisplayParentInt("Virtual Channel Count", settings.VirtualChannelSettings, FMODPlatform.Default, 0, 2048);
            DisplayParentInt("Real Channel Count", settings.RealChannelSettings, FMODPlatform.Default, 0, 2048);
            EditorGUI.indentLevel--;


            // ----- Plugins ----------------------------------------------
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("<b>Plugins</b>", GUI.skin.button, style);
            if (GUILayout.Button("Add Plugin", GUILayout.ExpandWidth(false)))
            {
                settings.Plugins.Add("");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            for (int count = 0; count < settings.Plugins.Count; count++)
            {
                EditorGUILayout.BeginHorizontal();
                settings.Plugins[count] = EditorGUILayout.TextField("Plugin " + (count + 1).ToString() + ":", settings.Plugins[count]);

                if (GUILayout.Button("Delete Plugin", GUILayout.ExpandWidth(false)))
                {
                    settings.Plugins.RemoveAt(count);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;


            // ----- Windows ----------------------------------------------
            DisplayPlatform(FMODPlatform.Desktop, null);
            DisplayPlatform(FMODPlatform.Mobile, new FMODPlatform[] { FMODPlatform.MobileHigh, FMODPlatform.MobileLow, FMODPlatform.PSVita });
            DisplayPlatform(FMODPlatform.Console, new FMODPlatform[] { FMODPlatform.XboxOne, FMODPlatform.PS4, FMODPlatform.WiiU });

            if (hasBankSourceChanged)
            {
                EditorUtils.CopyToStreamingAssets();
            }
        }

    }
}