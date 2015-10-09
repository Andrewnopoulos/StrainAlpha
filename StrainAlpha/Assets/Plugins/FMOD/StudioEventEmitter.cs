using UnityEngine;
using System;
using System.Collections.Generic;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Event Emitter")]
    public class StudioEventEmitter : MonoBehaviour
    {
        public EventRef Event;
        public BeginEvent PlayEvent;
        public EndEvent StopEvent;
        public String CollisionTag;
        public bool AllowFadeout = true;
        public bool TriggerOnce = false;

        private Collider triggerObject;

        private FMOD.Studio.EventDescription eventDescription;
        private FMOD.Studio.EventInstance instance;
        private bool hasTriggered;
        private Rigidbody cachedRigidBody;

        void Start() 
        {                        
            enabled = false;
            cachedRigidBody = GetComponent<Rigidbody>();

            if (PlayEvent == BeginEvent.LevelStart)
            {
                Play();
            }
        }

        void OnDestroy()
        {
            if (StopEvent == EndEvent.LevelEnd)
            {
                Stop();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (PlayEvent == BeginEvent.TriggerEnter &&
                triggerObject == null &&
                (String.IsNullOrEmpty(CollisionTag) || other.tag == CollisionTag))
            {
                triggerObject = other;
                Play();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (StopEvent == EndEvent.TriggerExit && other == triggerObject)
            {
                Stop();
                triggerObject = null;
            }
        }

        void Lookup()
        {
            eventDescription = RuntimeManager.GetEventDescription(Event);
        }

        public void Play()
        {
            if (TriggerOnce && hasTriggered)
            {
                return;
            }

            if (eventDescription == null)
            {
                Lookup();
            }

            eventDescription.createInstance(out instance);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
            instance.start();

            hasTriggered = true;
        }

        public void Stop()
        {
            if (instance != null)
            {
                instance.stop(AllowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
                instance = null;
            }
        }

        void Update()
        {
            if (instance != null)
            {
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
            }
        }

        void SetParameter(string name, float value)
        {
            if (instance != null)
            {
                instance.setParameterValue(name, value);
            }
        }
    }
}
