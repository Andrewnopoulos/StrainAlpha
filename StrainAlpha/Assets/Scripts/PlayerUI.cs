using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    private bool statsExpanded = false;
    private float expanding;

    private Vector3[] targetPos;
    private Image[] healthImages;

	// Use this for initialization
	void Start () {

        healthImages = new Image[8];
        targetPos = new Vector3[8];

        healthImages[0] = gameObject.GetComponentsInChildren<Image>()[0];
        healthImages[1] = gameObject.GetComponentsInChildren<Image>()[1];
        healthImages[2] = gameObject.GetComponentsInChildren<Image>()[2];
        healthImages[3] = gameObject.GetComponentsInChildren<Image>()[3];
        healthImages[4] = gameObject.GetComponentsInChildren<Image>()[4];
        healthImages[5] = gameObject.GetComponentsInChildren<Image>()[5];
        healthImages[6] = gameObject.GetComponentsInChildren<Image>()[6];
        healthImages[7] = gameObject.GetComponentsInChildren<Image>()[7];
	}
	
	// Update is called once per frame
	void Update () {

        expanding -= Time.deltaTime;

        if (Input.GetButtonDown("RightStickPress"))
        {
            ShowStats();
            statsExpanded = !statsExpanded;
        }

        if (expanding > 0)
        {
            for (int i = 0; i < 8; ++i)
            {
                Vector3 vel = Vector3.zero;
                healthImages[i].transform.localPosition = Vector3.SmoothDamp(healthImages[i].transform.localPosition, targetPos[i], ref vel, 0.05f);
            }
        }

	}

    void LateUpdate()
    {
        if (transform.rotation != Quaternion.Euler(90.0f, 0, 0))
        {
            transform.rotation = Quaternion.Euler(90.0f, 0, 0);
        }
    }

    void ShowStats()
    {
        if (!statsExpanded)
        {
            for (int i = 0; i < 8; ++i)
            {
                targetPos[i] = healthImages[i].transform.localPosition * 2;
            }
        }
        else
        {
            for (int i = 0; i < 8; ++i)
            {
                targetPos[i] = healthImages[i].transform.localPosition / 2;
            }
        }

        expanding = 1.0f;
    }
}
