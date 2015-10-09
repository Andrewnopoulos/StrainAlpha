using System;
using UnityEngine;
using System.Collections;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Listener")]
    public class Listener : MonoBehaviour
    {
        Rigidbody rigidBody;

        void Start()
        {
            rigidBody = gameObject.GetComponent<Rigidbody>();
            RuntimeManager.SetListenerLocation(gameObject, rigidBody);
        }

        void Update()
        {
            RuntimeManager.SetListenerLocation(gameObject, rigidBody);
        }
    }
}
