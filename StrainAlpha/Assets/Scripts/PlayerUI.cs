using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    private bool statsExpanded = false;
    private float expanding;

    private Vector3[] targetPos;
    private Image[] healthImages;

    private PlayerScript player;

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

        player = gameObject.GetComponentInParent<PlayerScript>();
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

        SetHealth();
	}

    void LateUpdate()
    {
        if (transform.rotation != Quaternion.Euler(90.0f, 0, 0))
        {
            transform.rotation = Quaternion.Euler(90.0f, 0, 0);
        }
    }

    void SetHealth()
    {
        if (player.GetHealth().x <= 0 && healthImages[0].enabled)
        {
            for (int i = 0; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * 0.125f && healthImages[1].enabled)
        {
            for (int i = 1; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 1; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * (0.125f * 2.0f) && healthImages[2].enabled)
        {
            for (int i = 2; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 2; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * (0.125f * 3.0f) && healthImages[3].enabled)
        {
            for (int i = 3; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 3; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * (0.125f * 4.0f) && healthImages[4].enabled)
        {
            for (int i = 4; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 4; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * (0.125f * 5.0f) && healthImages[5].enabled)
        {
            for (int i = 5; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 5; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * (0.125f * 6.0f) && healthImages[6].enabled)
        {
            for (int i = 6; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 6; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (player.GetHealth().x < player.GetHealth().z * (0.125f * 7.0f) && healthImages[7].enabled)
        {
            for (int i = 7; i < 8; ++i)
            {
                healthImages[i].enabled = false;
            }
            for (int i = 0; i < 7; ++i)
            {
                healthImages[i].enabled = true;
            }
        }
        else if (!healthImages[7].enabled)
        {
            for (int i = 0; i < 8; ++i)
            {
                healthImages[i].enabled = true;
            }
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
