using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor.Callbacks;

namespace FMODUnity
{
    [InitializeOnLoad]
    public class EventManager : MonoBehaviour
    {
        static EventCache eventCache;

        const string StringBankExtension = "strings.bank";
        const string BankExtension = "bank";
        const string DefaultBankPlatform = "Desktop";
        
        static void UpdateCache()
        {
            if (EditorUtils.GetBankDirectory() == null)
            {
                if (eventCache.StringsBankWriteTime != DateTime.MinValue)
                {
                    eventCache.StringsBankWriteTime = DateTime.MinValue;
                    eventCache.EditorBanks.Clear();
                    eventCache.EditorEvents.Clear();
                    OnCacheChange();
                }
                return;
            }

            string defaultBankFolder = null;
            
            if (!Settings.Instance.HasPlatforms)
            {
                defaultBankFolder = EditorUtils.GetBankDirectory();
            }
            else
            {
                defaultBankFolder = Path.Combine(EditorUtils.GetBankDirectory(), Settings.Instance.GetBankPlatform(FMODPlatform.PlayInEditor));
            }

            string[] bankPlatforms = EditorUtils.GetBankPlatforms();
            string[] bankFolders = new string[bankPlatforms.Length];            
            for (int i = 0; i < bankPlatforms.Length; i++)
            {
                bankFolders[i] = Path.Combine(EditorUtils.GetBankDirectory(), bankPlatforms[i]);
            }

            string[] stringBanks = new string[0];
            try
            {
                stringBanks = Directory.GetFiles(defaultBankFolder, "*." + StringBankExtension);
            }
            catch
            {
            }

            if (stringBanks.Length == 0)
            {
                if (eventCache.StringsBankWriteTime != DateTime.MinValue)
                {
                    eventCache.StringsBankWriteTime = DateTime.MinValue;
                    eventCache.EditorBanks.Clear();
                    eventCache.EditorEvents.Clear();
                    OnCacheChange();
                    UnityEngine.Debug.LogWarningFormat("FMOD Studio: Directory {0} doesn't contain any banks. Build from the tool or check the path in the settings", defaultBankFolder);
                }
                return;
            }

            // If we have multiple .strings.bank files find the most recent
            Array.Sort(stringBanks, (a, b) => File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)));
            string stringBankPath = stringBanks[0];

            // Use the string bank timestamp as a marker for the most recent build of any bank because it gets exported every time
            if (File.GetLastWriteTime(stringBankPath) <= eventCache.StringsBankWriteTime)
            {
                return;
            }
            
            eventCache.StringsBankWriteTime = File.GetLastWriteTime(stringBankPath);

            string masterBankFileName = Path.GetFileName(stringBankPath).Replace(StringBankExtension, BankExtension);

            FMOD.Studio.Bank stringBank = null;
            EditorUtils.CheckResult(EditorUtils.System.loadBankFile(stringBankPath, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out stringBank));
            if (stringBank == null)
            {
                if (eventCache.StringsBankWriteTime != DateTime.MinValue)
                {
                    eventCache.StringsBankWriteTime = DateTime.MinValue;
                    eventCache.EditorBanks.Clear();
                    eventCache.EditorEvents.Clear();
                    OnCacheChange();
                }
                return;
            }

            // Iterate every string in the strings bank and look for any that identify banks
            int stringCount;
            stringBank.getStringCount(out stringCount);
            List<string> bankFileNames = new List<string>();
            for (int stringIndex = 0; stringIndex < stringCount; stringIndex++)
            {
                string currentString;
                Guid currentGuid;
                stringBank.getStringInfo(stringIndex, out currentGuid, out currentString);
                const string BankPrefix = "bank:/";
                int BankPrefixLength = BankPrefix.Length;
                if (currentString.StartsWith(BankPrefix))
                {
                    string bankFileName = currentString.Substring(BankPrefixLength) + "." + BankExtension;
                    if (!bankFileName.Contains(StringBankExtension)) // filter out the strings bank
                    {
                        bankFileNames.Add(bankFileName);
                    }
                }
            }

            eventCache.EditorBanks.ForEach((x) => x.Exists = false);

            foreach (string bankFileName in bankFileNames)
            {
                string bankPath = Path.Combine(defaultBankFolder, bankFileName);
                EditorBankRef bankRef = eventCache.EditorBanks.Find((x) => bankPath == x.Path);

                // New bank we've never seen before
                if (bankRef == null)
                {
                    bankRef = ScriptableObject.CreateInstance<EditorBankRef>();
                    AssetDatabase.AddObjectToAsset(bankRef, CacheAssetFullName);
                    bankRef.Path = bankPath;
                    bankRef.LastModified = DateTime.MinValue;
                    bankRef.FileSizes = new List<EditorBankRef.NameValuePair>();
                    eventCache.EditorBanks.Add(bankRef);
                }

                bankRef.Exists = true;
                
                // Timestamp check - if it doesn't match update events from that bank
                if (bankRef.LastModified != File.GetLastWriteTime(bankPath))
                {
                    bankRef.LastModified = File.GetLastWriteTime(bankPath);                    
                    UpdateCacheBank(bankRef);
                }

                // Update file sizes
                bankRef.FileSizes.Clear();      
                for (int i = 0; i < bankPlatforms.Length; i++)
                {		
                    string platformBankPath = Path.Combine(bankFolders[i], bankFileName);
                    var fileInfo = new FileInfo(platformBankPath);
                    if (fileInfo.Exists)
                    {
                        bankRef.FileSizes.Add(new EditorBankRef.NameValuePair(bankPlatforms[i], fileInfo.Length));
                    }
                }

                if (bankFileName == masterBankFileName)
                {
                    eventCache.MasterBankRef = bankRef;
                }
            }

            // Unload the strings bank
            stringBank.unload();

            // Remove any stale entries from bank and event lists
            eventCache.EditorBanks.FindAll((x) => !x.Exists).ForEach(RemoveCacheBank);
            eventCache.EditorBanks.RemoveAll((x) => !x.Exists);
            eventCache.EditorEvents.RemoveAll((x) => x.Banks.Count == 0);

            OnCacheChange();
        }

        static void UpdateCacheBank(EditorBankRef bankRef)
        {
            // Clear out any cached events from this bank
            eventCache.EditorEvents.ForEach((x) => x.Banks.Remove(bankRef));

            FMOD.Studio.Bank bank;
            bankRef.LoadResult = EditorUtils.System.loadBankFile(bankRef.Path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out bank);

            if (bankRef.LoadResult == FMOD.RESULT.ERR_EVENT_ALREADY_LOADED)
            {
                EditorUtils.System.getBank(bankRef.Name, out bank);
                bank.unload();
                bankRef.LoadResult = EditorUtils.System.loadBankFile(bankRef.Path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out bank);
            }

            if (bankRef.LoadResult == FMOD.RESULT.OK)
            {
                // Iterate all events in the bank and cache them
                FMOD.Studio.EventDescription[] eventList;
                var result = bank.getEventList(out eventList);
                if (result == FMOD.RESULT.OK)
                {
                    foreach (var eventDesc in eventList)
                    {
                        string path;
                        eventDesc.getPath(out path);
                        EditorEventRef eventRef = eventCache.EditorEvents.Find((x) => x.Path == path);
                        if (eventRef == null)
                        {
                            eventRef = ScriptableObject.CreateInstance<EditorEventRef>();
                            AssetDatabase.AddObjectToAsset(eventRef, CacheAssetFullName);
                            eventRef.Banks = new List<EditorBankRef>();
                            eventCache.EditorEvents.Add(eventRef);
                        }

                        eventRef.Banks.Add(bankRef);
                        Guid guid;
                        eventDesc.getID(out guid);
                        eventRef.Guid = guid;
                        eventRef.Path = path;
                        eventDesc.is3D(out eventRef.Is3D);
                        eventDesc.isOneshot(out eventRef.IsOneShot);
                        eventDesc.isStream(out eventRef.IsStream);
                        eventDesc.getMaximumDistance(out eventRef.MaxDistance);
                        eventDesc.getMinimumDistance(out eventRef.MinDistance);
                        int paramCount = 0;
                        eventDesc.getParameterCount(out paramCount);
                        eventRef.Parameters = new List<EditorParamRef>(paramCount);
                        for (int paramIndex = 0; paramIndex < paramCount; paramIndex++)
                        {
                            EditorParamRef paramRef = ScriptableObject.CreateInstance<EditorParamRef>();
                            AssetDatabase.AddObjectToAsset(paramRef, CacheAssetFullName);
                            FMOD.Studio.PARAMETER_DESCRIPTION param;
                            eventDesc.getParameterByIndex(paramIndex, out param);
                            paramRef.Name = param.name;
                            paramRef.Min = param.minimum;
                            paramRef.Max = param.maximum;
                            eventRef.Parameters.Add(paramRef);
                        }
                    }
                }

                bank.unload();
            }
            else
            {
                // TODO: log the error
                //UnityEngine.Debug.LogWarning("Cannot load );
            }
        }

        static void RemoveCacheBank(EditorBankRef bankRef)
        {
            eventCache.EditorEvents.ForEach((x) => x.Banks.Remove(bankRef));
        }


        const string CacheAssetName = "FMODStudioCache";
        const string CacheAssetFullName = "Assets/Resources/" + CacheAssetName + ".asset";

        static EventManager()
	    {
            eventCache = Resources.Load(CacheAssetName) as EventCache;
            if (eventCache == null)
            {
                UnityEngine.Debug.Log("FMOD Studio: Cannot find serialized event cache, creating new instance");
                eventCache = ScriptableObject.CreateInstance<EventCache>();

                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateAsset(eventCache, CacheAssetFullName);
            }

            EditorUserBuildSettings.activeBuildTargetChanged += BuildTargetChanged;
            EditorApplication.update += Update;
	    }     

        /*[MenuItem("FMOD/Show Banks")]
        public static void ShowBanks()
        {
            UpdateCache();
            StringBuilder message = new StringBuilder();
            foreach(var bankRef in eventCache.EditorBanks)
            {
                message.AppendLine(String.Format("{0}\t\t{1}", Path.GetFileNameWithoutExtension(bankRef.Path), bankRef.LoadResult.ToString()));
            }
            EditorUtility.DisplayDialog("Loaded Banks", message.ToString(), "OK");
        }

        [MenuItem("FMOD/Force Import")]
        public static void ForceImport()
        {
            eventCache = ScriptableObject.CreateInstance<EventCache>();
            AssetDatabase.CreateAsset(eventCache, "Assets/Resources/" + CacheAssetName + ".asset");
            UpdateCache();
        }

        [MenuItem("FMOD/Force Cache Reload")]
        public static void ForceReload()
        {
            eventCache = Resources.Load(CacheAssetName) as EventCache;
            UpdateCache();
        }*/

        private static void BuildTargetChanged()
        {
            UpdateCache();

            // Copy over assets for the new platform
            EditorUtils.CopyToStreamingAssets();
        }   

        static void OnCacheChange()
        {
            Settings.Instance.MasterBank = new BankRef(MasterBank.Name);

            Settings.Instance.Banks.Clear();
            foreach (var bankRef in eventCache.EditorBanks)
            {
                if (bankRef != MasterBank)
                {
                    Settings.Instance.Banks.Add(new BankRef(bankRef.Name));
                }
            }
            EditorUtility.SetDirty(Settings.Instance);
            EditorUtility.SetDirty(eventCache);
			
			EditorUtils.CopyToStreamingAssets();
            
            EventBrowser eventBrowser = EditorWindow.GetWindow<EventBrowser>("FMOD Events");
            eventBrowser.Repaint();
        }

        static float lastCheckTime;
        static void Update()
        {
            if (lastCheckTime + 10 < Time.realtimeSinceStartup)
            {
                UpdateCache();
                lastCheckTime = Time.time;
            }
        }

        public static List<EditorEventRef> Events
        {
            get
            {
                UpdateCache();
                return eventCache.EditorEvents;
            }
        }

        public static List<EditorBankRef> Banks
        {
            get
            {
                UpdateCache();
                return eventCache.EditorBanks;
            }
        }

        public static EditorBankRef MasterBank
        { 
            get
            {
                UpdateCache();
                return eventCache.MasterBankRef;
            }
        }

        public static bool IsLoaded
        {
            get
            {
                return EditorUtils.GetBankDirectory() != null;
            }
        }

        public static EditorEventRef EventFromPath(string path)
        {
            UpdateCache();
            return eventCache.EditorEvents.Find((x) => x.Path == path);
        }

        public static EditorEventRef EventFromGUID(Guid guid)
        {
            UpdateCache();
            return eventCache.EditorEvents.Find((x) => x.Guid == guid);
        }
    }

}
