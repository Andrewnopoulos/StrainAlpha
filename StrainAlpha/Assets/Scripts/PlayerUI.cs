﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {

    private bool statsExpanded = false;
    private float expanding;

    private Vector3[] targetPos;
    private float currentAlpha;
    private float targetAlpha;

    private Image[] allImages;

    private Image[] healthImages;

    private Image healthGene;
    private Image rangeGene;
    private Image damageGene;
    private Image speedGene;

    private float previousHealth;
    private float previousRange;
    private float previousDamage;
    private float previousSpeed;

    private PlayerScript player;

    private GameObject anchor;

    private Camera mainCamera;

	// Use this for initialization
	void Start () {

        allImages = new Image[16];

        healthImages = new Image[8];
        targetPos = new Vector3[12];

        allImages = gameObject.GetComponentsInChildren<Image>();

        healthImages[0] = allImages[0];
        healthImages[1] = allImages[1];
        healthImages[2] = allImages[2];
        healthImages[3] = allImages[3];
        healthImages[4] = allImages[4];
        healthImages[5] = allImages[5];
        healthImages[6] = allImages[6];
        healthImages[7] = allImages[7];

        healthGene = allImages[9];
        rangeGene = allImages[11];
        damageGene = allImages[13];
        speedGene = allImages[15];

        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        anchor = GameObject.Find("PlayerAnchor");

        mainCamera = Camera.allCameras[0];

        SetGenes();
	}
	
	// Update is called once per frame
	void Update () {

        expanding -= Time.deltaTime;

        if (Input.GetButtonDown("RightStickPress") || Input.GetMouseButtonDown(2))
        {
            ShowStats();
            statsExpanded = !statsExpanded;
        }

        if (expanding > 0)
        {

            if (targetAlpha == 0.0f)
            {
                currentAlpha = expanding * targetAlpha;
            }
            else
            {
                currentAlpha = (1.0f - expanding) * targetAlpha;
            }

            for (int i = 0; i < 8; ++i)
            {
                Vector3 vel = Vector3.zero;
                healthImages[i].transform.localPosition = Vector3.SmoothDamp(healthImages[i].transform.localPosition, targetPos[i], ref vel, 0.05f);
            }

            //---------------------- expand genes
        }
        else
        {
            currentAlpha = targetAlpha;
        }

        SetHealth();
        SetGenes();

	}

    void LateUpdate()
    {
        anchor.transform.position = mainCamera.WorldToScreenPoint(player.transform.position);
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

    void SetGenes()
    {
        healthGene.transform.RotateAround(anchor.transform.position, transform.forward, (previousHealth - player.GetGene(0)) * 90);
        rangeGene.transform.RotateAround(anchor.transform.position, transform.forward, (player.GetGene(2) - previousRange) * 90);
        damageGene.transform.RotateAround(anchor.transform.position, transform.forward, (previousDamage - player.GetGene(1)) * 90);
        speedGene.transform.RotateAround(anchor.transform.position, transform.forward, (player.GetGene(3) - previousSpeed) * 90);

        previousHealth = player.GetGene(0);
        previousRange = player.GetGene(2);
        previousDamage = player.GetGene(1);
        previousSpeed = player.GetGene(3);
       
    }

    void ShowStats()
    {
        if (!statsExpanded)
        {
            for (int i = 0; i < 8; ++i)
            {
                targetPos[i] = healthImages[i].transform.localPosition * 2;
            }

            targetAlpha = 0.5f;
        }
        else
        {
            for (int i = 0; i < 8; ++i)
            {
                targetPos[i] = healthImages[i].transform.localPosition / 2;
            }

            targetAlpha = 0.0f;
        }

        expanding = 1.0f;
    }
}
