using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Net.Sockets;

namespace FMODUnity
{
    [InitializeOnLoad]
    class EditorUtils : MonoBehaviour
    {
        public static void CheckResult(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                UnityEngine.Debug.LogError(string.Format("FMOD Studio: Encounterd Error: {0} {1}", result, FMOD.Error.String(result)));
            }
        }

        const string BuildFolder = "Build";

        public static string GetBankDirectory()
        {
            if (Settings.Instance.HasSourceProject && !String.IsNullOrEmpty(Settings.Instance.SourceProjectPath))
            {
                string projectPath = Settings.Instance.SourceProjectPath;
                string projectFolder = Path.GetDirectoryName(projectPath);
                return Path.Combine(projectFolder, BuildFolder);            
            }
            else if (!String.IsNullOrEmpty(Settings.Instance.SourceBankPath))
            {
                return Settings.Instance.SourceBankPath;
            }
            return null;
        }

        public static string[] GetBankPlatforms()
        {
            string buildFolder = GetBankDirectory();
            try
            {
                if (Directory.GetFiles(buildFolder, "*.bank").Length == 0)
                {                
                    string[] buildDirectories = Directory.GetDirectories(buildFolder);
                    string[] buildNames = new string[buildDirectories.Length];
                    for (int i = 0; i < buildDirectories.Length; i++)
                    {
                        buildNames[i] = Path.GetFileNameWithoutExtension(buildDirectories[i]);
                    }
                    return buildNames;
                }
            }
            catch
            {
            }
            return new string[0];
        }

        public static FMODPlatform GetFMODPlatform(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return FMODPlatform.Android;
                case BuildTarget.iOS:
                    return FMODPlatform.iOS;
                case BuildTarget.PS4:
                    return FMODPlatform.PS4;
                case BuildTarget.PSP2:
                    return FMODPlatform.PSVita;
                case BuildTarget.StandaloneLinux:
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneLinuxUniversal:
                    return FMODPlatform.Linux;
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return FMODPlatform.Mac;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return FMODPlatform.Windows;
                case BuildTarget.XboxOne:
                    return FMODPlatform.XboxOne;
                case BuildTarget.WSAPlayer:
                    return FMODPlatform.WindowsPhone; // TODO: not correct if we support Win RT
                default:
                    return FMODPlatform.None;
            }
        }

        static string VerionNumberToString(uint version)
        {
            uint major = (version & 0x00FF0000) >> 16;
            uint minor = (version & 0x0000FF00) >> 8;
            uint patch = (version & 0x000000FF);

            return major.ToString("X1") + "." + minor.ToString("X2") + "." + patch.ToString("X2");
        }

        static EditorUtils()
	    {
            EditorApplication.update += Update;
		    EditorApplication.playmodeStateChanged += HandleOnPlayModeChanged;
	    }
 
	    static void HandleOnPlayModeChanged()
	    {
            // Ensure we don't leak system handles in the DLL
		    if (EditorApplication.isPlayingOrWillChangePlaymode &&
			    !EditorApplication.isPaused)
		    {
        	    DestroySystem();
		    }
	    }

        static void Update()
        {
            // Ensure we don't leak system handles in the DLL
            if (EditorApplication.isCompiling)
            {
                DestroySystem();
            }

            // Update the editor system
            if (system != null && system.isValid())
            {
                CheckResult(system.update());
            }
        }

        static FMOD.Studio.System system;

        static void DestroySystem()
        {
            if (system != null)
            {
                UnityEngine.Debug.Log("FMOD Studio: Destroying editor system instance");
                system.release();
                system = null;
            }
        }

        static void CreateSystem()
        {
            UnityEngine.Debug.Log("FMOD Studio: Creating editor system instance");
            RuntimeUtils.EnforceLibraryOrder();

            CheckResult(FMOD.Studio.System.create(out system));

            FMOD.System lowlevel;
            CheckResult(system.getLowLevelSystem(out lowlevel));

            // Use play-in-editor speaker mode for event browser preview and metering
            lowlevel.setSoftwareFormat(0, (FMOD.SPEAKERMODE)Settings.Instance.GetSpeakerMode(FMODPlatform.PlayInEditor), 0);

            CheckResult(system.initialize(256, FMOD.Studio.INITFLAGS.ALLOW_MISSING_PLUGINS | FMOD.Studio.INITFLAGS.SYNCHRONOUS_UPDATE, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));

            FMOD.ChannelGroup master;
            CheckResult(lowlevel.getMasterChannelGroup(out master));
            FMOD.DSP masterHead;
            CheckResult(master.getDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, out masterHead));
            CheckResult(masterHead.setMeteringEnabled(false, true));
        }

        public static FMOD.Studio.System System
        {
            get
            {
                if (system == null)
                {
                    CreateSystem();
                }
                return system;
            }
        }
        
        [MenuItem("FMOD/About Integration")]
        public static void About()
        {
            FMOD.System lowlevel;
            CheckResult(System.getLowLevelSystem(out lowlevel));

            uint version;
            CheckResult(lowlevel.getVersion(out version));

            EditorUtility.DisplayDialog("FMOD Studio Unity Integration", "Version: " + VerionNumberToString(version), "OK");
        }

        static FMOD.Studio.Bank masterBank;
        static FMOD.Studio.Bank previewBank;
        static FMOD.Studio.EventDescription previewEventDesc;
        static FMOD.Studio.EventInstance previewEventInstance;

        public static void PreviewEvent(EditorEventRef eventRef)
        {
            bool load = true;
            if (previewEventDesc != null)
            {
                Guid guid;
                previewEventDesc.getID(out guid);
                if (guid == eventRef.Guid)
                {
                    load = false;
                }
                else
                {
                    PreviewStop();
                }
            }

            if (load)
            {
                CheckResult(System.loadBankFile(EventManager.MasterBank.Path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out masterBank));
                CheckResult(System.loadBankFile(eventRef.Banks[0].Path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out previewBank));

                CheckResult(System.getEventByID(eventRef.Guid, out previewEventDesc));
                CheckResult(previewEventDesc.createInstance(out previewEventInstance));
            }

            CheckResult(previewEventInstance.start());
        }

        public static void PreviewUpdateParameter(string paramName, float paramValue)
        {
            if (previewEventInstance != null)
            {
                CheckResult(previewEventInstance.setParameterValue(paramName, paramValue));
            }
        }

        public static void PreviewUpdatePosition(float distance, float orientation)
        {
            if (previewEventInstance != null)
            {
                // Listener at origin
                FMOD.ATTRIBUTES_3D pos = new FMOD.ATTRIBUTES_3D();
                pos.position.x = (float)Math.Sin(orientation) * distance;
                pos.position.y = (float)Math.Cos(orientation) * distance;
                pos.forward.x = 1.0f;
                pos.up.z = 1.0f;
                CheckResult(previewEventInstance.set3DAttributes(pos));
            }
        }

        public static void PreviewPause()
        {
            if (previewEventInstance != null)
            {
                bool paused;
                CheckResult(previewEventInstance.getPaused(out paused));
                CheckResult(previewEventInstance.setPaused(!paused));
            }
        }

        public static void PreviewStop()
        {
            if (previewEventInstance != null)
            {
                previewEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                previewEventInstance.release();
                previewEventInstance = null;
                previewEventDesc = null;
                previewBank.unload();
                masterBank.unload();
            }
        }

        public static float[] GetMetering()
        {
            FMOD.System lowlevel;
            CheckResult(System.getLowLevelSystem(out lowlevel));
            FMOD.ChannelGroup master;
            CheckResult(lowlevel.getMasterChannelGroup(out master));
            FMOD.DSP masterHead;
            CheckResult(master.getDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, out masterHead));

            FMOD.DSP_METERING_INFO inputMetering = null;
            FMOD.DSP_METERING_INFO outputMetering = new FMOD.DSP_METERING_INFO();
            CheckResult(masterHead.getMeteringInfo(inputMetering, outputMetering));

            float[] data = new float[outputMetering.numchannels];
            Array.Copy(outputMetering.rmslevel, data, outputMetering.numchannels);
            return data;
        }

        public static void CopyToStreamingAssets()
        {
            FMODPlatform platform = EditorUtils.GetFMODPlatform(EditorUserBuildSettings.activeBuildTarget);
            if (platform == FMODPlatform.None)
            {
                UnityEngine.Debug.LogWarningFormat("FMOD Studio: copy banks for platform {0} : Unsupported platform", EditorUserBuildSettings.activeBuildTarget.ToString());
                return;
            }

            string bankTargetFolder = Application.dataPath + "/StreamingAssets";
            Directory.CreateDirectory(bankTargetFolder);

            string bankSourceFolder = EditorUtils.GetBankDirectory() + "/" + Settings.Instance.GetBankPlatform(platform);

            if (Path.GetFullPath(bankTargetFolder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant() == 
                Path.GetFullPath(bankSourceFolder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant())
            {
                return;
            }

            UnityEngine.Debug.LogFormat("FMOD Studio: copy banks for platform {0} : copying banks from {1} to {2}", platform.ToString(), bankSourceFolder, bankTargetFolder);

            // Clean out any stale .bank files
            string[] currentBankFiles = Directory.GetFiles(bankTargetFolder, "*.bank");
            foreach (var bankFileName in currentBankFiles)
            {
                if (!EventManager.Banks.Exists((x) => Path.GetFileNameWithoutExtension(bankFileName) == x.Name))
                {
                    File.Delete(bankFileName);
                }
            }

            // Copy over any files that don't match timestamp or size or don't exist
            foreach (var bankRef in EventManager.Banks)
            {
                string sourcePath = bankSourceFolder + "/" + bankRef.Name + ".bank";
                string targetPath = bankTargetFolder + "/" + bankRef.Name + ".bank";

                FileInfo sourceInfo = new FileInfo(sourcePath);
                FileInfo targetInfo = new FileInfo(targetPath);

                if (!targetInfo.Exists ||
                    sourceInfo.Length != targetPath.Length ||
                    sourceInfo.LastWriteTime != targetInfo.LastWriteTime)
                {
                    File.Copy(sourcePath, targetPath, true);

                    if (bankRef == EventManager.MasterBank)
                    {
                        sourcePath = bankSourceFolder + "/" + bankRef.Name + ".strings.bank";
                        targetPath = bankTargetFolder + "/" + bankRef.Name + ".strings.bank";
                        File.Copy(sourcePath, targetPath, true);
                    }
                }
            }
        }

        const int StudioScriptPort = 3663;
        static NetworkStream networkStream = null;

        static NetworkStream ScriptStream
        {
            get
            {
                if (networkStream == null)
                {
                    try
                    {
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect("127.0.0.1", StudioScriptPort);
                        networkStream = new NetworkStream(socket);

                        byte[] headerBytes = new byte[128];
                        int read = ScriptStream.Read(headerBytes, 0, 128);
                        string header = Encoding.UTF8.GetString(headerBytes, 0, read - 1);
                        if (header.StartsWith("log():"))
                        {
                            UnityEngine.Debug.Log("FMOD Studio: Script Client returned " + header.Substring(6));
                        }    
                    }        
                    catch (Exception e)
                    {
                        UnityEngine.Debug.Log("FMOD Studio: Script Client failed to connect - Check FMOD Studio is running");
                        throw e;
                    }
                }
                return networkStream;
            }
        }

        public static bool SendScriptCommand(string command)
        {
            byte[] commandBytes = Encoding.UTF8.GetBytes(command);
            try
            {
                ScriptStream.Write(commandBytes, 0, commandBytes.Length);
                byte[] commandReturnBytes = new byte[128];
                int read = ScriptStream.Read(commandReturnBytes, 0, 128);
                string result = Encoding.UTF8.GetString(commandReturnBytes, 0, read - 1);
                return (result.Contains("true"));
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("FMOD Studio: Script Client failed to connect - Check FMOD Studio is running");
                return false;
            }
        }
    }
}