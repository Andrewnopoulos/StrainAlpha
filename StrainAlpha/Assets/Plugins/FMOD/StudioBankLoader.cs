using UnityEngine;
using System;
using System.Collections.Generic;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Bank Loader")]
    public class StudioBankLoader : MonoBehaviour
    {

        public BeginEvent LoadEvent;
        public EndEvent UnloadEvent;
        public BankRefList Banks;
        public String CollisionTag;
        public bool PreloadSamples;

        Collider triggerObject;
        bool loaded;
      
        void OnEnable()
        {
            if (LoadEvent == BeginEvent.LevelStart)
            {
                Load();
                loaded = true;
            }
        }

        void OnApplicationQuit()
        {
            loaded = false;
        }

        void OnDisable()
        {
            if (UnloadEvent == EndEvent.LevelEnd && loaded)
            {
                Unload();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (LoadEvent == BeginEvent.TriggerEnter && 
                triggerObject == null &&
                (String.IsNullOrEmpty(CollisionTag) || other.tag == CollisionTag))
            {
                triggerObject = other;
                Load();
                loaded = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (UnloadEvent == EndEvent.TriggerExit && other == triggerObject && loaded)
            {
                Unload();
                triggerObject = null;
            }
        }

        public void Load()
        {
            foreach (var bankRef in Banks)
            {
                try
                {
                    RuntimeManager.LoadBank(bankRef, PreloadSamples);
                }
                catch (BankLoadException e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        public void Unload()
        {
            foreach (var bankRef in Banks)
            {
                RuntimeManager.UnloadBank(bankRef);
            }
        }
    }
}
