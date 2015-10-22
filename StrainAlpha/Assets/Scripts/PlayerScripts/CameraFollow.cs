using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject player;

    private bool screenShake = false;
    private float axisShake = 2.0f;

    private float shakeHoldTime = 0.02f;
    private float shakeCooldown = 0;
    private Vector3 shakeOffset;

    private Vector3 goalPos;

    private float shakeLength = 0.2f;
    private float currentShakeLength = 0.1f;

    public float smoothTime = 0.15f;

    private Vector3 targetPos;
    private Quaternion targetRot;

    private Vector3 closeUpPos;
    private Quaternion closeUpRot;

    private Vector3 gamePos;
    private Quaternion gameRot;

    private Vector3 bossPos;
    private Quaternion bossRot;

	// Use this for initialization
	void Start () {

        gamePos = new Vector3(0, 20.5f, 0);
        gameRot = Quaternion.Euler(90.0f, 0, 0);

        bossPos = new Vector3(0, 25.0f, 0);
        bossRot = Quaternion.Euler(90.0f, 0, 0);

        closeUpPos = new Vector3(0, 7.0f, -2.0f);
        closeUpRot = Quaternion.Euler(60.0f, 0, 0);

        targetRot = closeUpRot;
        targetPos = closeUpPos;

        transform.position = player.transform.position + targetPos;
	}

    public void ShakeScreen()
    {
        screenShake = true;
    }
	
	// Update is called once per frame
	void Update () 
    {
        //playerDistance += (targetDist - playerDistance) * (Time.deltaTime);

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

            goalPos = player.transform.position + targetPos;
            goalPos.x += shakeOffset.x;
            goalPos.z += shakeOffset.z;
            transform.position = Vector3.Lerp(transform.position, goalPos, smoothTime);
        }
        else
        {
            goalPos = player.transform.position + targetPos;
            transform.position = Vector3.Lerp(transform.position, goalPos, smoothTime);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, smoothTime);
	}

    public void SetPlayerDist(int stage)
    {
        if (stage == 0)
        {
            targetRot = closeUpRot;
            targetPos = closeUpPos;
        }
        else if (stage == 1)
        {
            targetRot = gameRot;
            targetPos = gamePos;
        }
        else if (stage == 2)
        {
            targetRot = bossRot;
            targetPos = bossPos;
        }
        else
        {
            //invalid input
            SetPlayerDist(1);
        }
    }
}
