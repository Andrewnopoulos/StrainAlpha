using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;

    private bool screenShake = false;
    private float axisShake = 2.0f;

    private float shakeHoldTime = 0.02f;
    private float shakeCooldown = 0;
    private Vector3 shakeOffset;

    private float shakeLength = 0.2f;
    private float currentShakeLength = 0.1f;

    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.15f;

	// Use this for initialization
	void Start () {

	}

    public void ShakeScreen()
    {
        screenShake = true;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (screenShake)
        {
            currentShakeLength -= Time.deltaTime;

            if (currentShakeLength < 0)
            {
                currentShakeLength = shakeLength;
                screenShake = false;
            }

            shakeCooldown -= Time.deltaTime;

            if (shakeCooldown < 0)
            {
                shakeOffset = new Vector3(Random.Range(-axisShake, axisShake), 0, Random.Range(-axisShake, axisShake));
                shakeCooldown = shakeHoldTime;
            }

            Vector3 goalPos = player.transform.position;
            goalPos.x += shakeOffset.x;
            goalPos.y = transform.position.y;
            goalPos.z += shakeOffset.z;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        }
        else
        {
            Vector3 goalPos = player.transform.position;
            goalPos.y = transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        }
	}
}
