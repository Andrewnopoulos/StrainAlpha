using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    private bool statsExpanded = false;
    private float expanding;

    private Vector3[] targetPos;
    private float targetTrans;
    private Image[] healthImages;

    private Image[] geneImages;

    private PlayerScript player;

	// Use this for initialization
	void Start () {

        healthImages = new Image[8];
        targetPos = new Vector3[12];
        geneImages = new Image[4];

        healthImages[0] = gameObject.GetComponentsInChildren<Image>()[0];
        healthImages[1] = gameObject.GetComponentsInChildren<Image>()[1];
        healthImages[2] = gameObject.GetComponentsInChildren<Image>()[2];
        healthImages[3] = gameObject.GetComponentsInChildren<Image>()[3];
        healthImages[4] = gameObject.GetComponentsInChildren<Image>()[4];
        healthImages[5] = gameObject.GetComponentsInChildren<Image>()[5];
        healthImages[6] = gameObject.GetComponentsInChildren<Image>()[6];
        healthImages[7] = gameObject.GetComponentsInChildren<Image>()[7];

        geneImages[0] = gameObject.GetComponentsInChildren<Image>()[8];
        geneImages[1] = gameObject.GetComponentsInChildren<Image>()[9];
        geneImages[2] = gameObject.GetComponentsInChildren<Image>()[10];
        geneImages[3] = gameObject.GetComponentsInChildren<Image>()[11];

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

            for (int i = 0; i < 4; ++i)
            {
                Vector3 velocity = Vector3.zero;
                geneImages[i].transform.localPosition = Vector3.SmoothDamp(geneImages[i].transform.localPosition, targetPos[i + 8], ref velocity, 0.05f);
                geneImages[i].color = Color.Lerp(new Color(geneImages[i].color.r, geneImages[i].color.g, geneImages[i].color.b, Mathf.Abs(targetTrans - 0.3f)),
                    new Color(geneImages[i].color.r, geneImages[i].color.g, geneImages[i].color.b, Mathf.Abs(0 + targetTrans)), 1 - (expanding / 2));
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
        else if (player.GetHealth().x >= player.GetHealth().z && !healthImages[7].enabled)
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
            targetPos[8] = new Vector3(-106.0f, 106.0f, 0);
            targetPos[9] = new Vector3(-106.0f, -106.0f, 0);
            targetPos[10] = new Vector3(106.0f, -106.0f, 0);
            targetPos[11] = new Vector3(106.0f, 106.0f, 0);

            targetTrans = 0.3f;
        }
        else
        {
            for (int i = 0; i < 8; ++i)
            {
                targetPos[i] = healthImages[i].transform.localPosition / 2;
            }
            targetPos[8] = new Vector3(0, 0, 0);
            targetPos[9] = new Vector3(0, 0, 0);
            targetPos[10] = new Vector3(0, 0, 0);
            targetPos[11] = new Vector3(0, 0, 0);

            targetTrans = 0;
        }

        expanding = 1.0f;
    }
}
