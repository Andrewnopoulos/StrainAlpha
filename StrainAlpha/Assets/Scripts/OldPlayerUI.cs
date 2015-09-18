using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OldPlayerUI : MonoBehaviour
{

    private bool statsExpanded = false;
    private float expanding;

    private Vector3[] targetPos;
    private float currentAlpha;
    private float targetAlpha;

    private Image[] allImages;

    private Image[] healthImages;

    private Image[] healthGenes;
    private Image[] rangeGenes;
    private Image[] damageGenes;
    private Image[] speedGenes;

    private PlayerScript player;

    // Use this for initialization
    void Start()
    {

        allImages = new Image[28];

        healthImages = new Image[8];
        targetPos = new Vector3[28];
        healthGenes = new Image[5];
        rangeGenes = new Image[5];
        damageGenes = new Image[5];
        speedGenes = new Image[5];

        allImages = gameObject.GetComponentsInChildren<Image>();

        healthImages[0] = allImages[0];
        healthImages[1] = allImages[1];
        healthImages[2] = allImages[2];
        healthImages[3] = allImages[3];
        healthImages[4] = allImages[4];
        healthImages[5] = allImages[5];
        healthImages[6] = allImages[6];
        healthImages[7] = allImages[7];

        healthGenes[0] = allImages[8];
        healthGenes[1] = allImages[9];
        healthGenes[2] = allImages[10];
        healthGenes[3] = allImages[11];
        healthGenes[4] = allImages[12];

        rangeGenes[0] = allImages[13];
        rangeGenes[1] = allImages[14];
        rangeGenes[2] = allImages[15];
        rangeGenes[3] = allImages[16];
        rangeGenes[4] = allImages[17];

        damageGenes[0] = allImages[18];
        damageGenes[1] = allImages[19];
        damageGenes[2] = allImages[20];
        damageGenes[3] = allImages[21];
        damageGenes[4] = allImages[22];

        speedGenes[0] = allImages[23];
        speedGenes[1] = allImages[24];
        speedGenes[2] = allImages[25];
        speedGenes[3] = allImages[26];
        speedGenes[4] = allImages[27];

        player = gameObject.GetComponentInParent<PlayerScript>();

        SetGenes();
    }

    // Update is called once per frame
    void Update()
    {

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

            for (int i = 0; i < 5; ++i)
            {
                Vector3 velocity = Vector3.zero;
                healthGenes[i].transform.localPosition = Vector3.SmoothDamp(healthGenes[i].transform.localPosition, targetPos[i + 8], ref velocity, 0.05f);
                healthGenes[i].color = new Color(healthGenes[i].color.r, healthGenes[i].color.g, healthGenes[i].color.b, currentAlpha);
            }
            for (int i = 0; i < 5; ++i)
            {
                Vector3 velocity = Vector3.zero;
                rangeGenes[i].transform.localPosition = Vector3.SmoothDamp(rangeGenes[i].transform.localPosition, targetPos[i + 13], ref velocity, 0.05f);
                rangeGenes[i].color = new Color(rangeGenes[i].color.r, rangeGenes[i].color.g, rangeGenes[i].color.b, currentAlpha);
            }
            for (int i = 0; i < 5; ++i)
            {
                Vector3 velocity = Vector3.zero;
                damageGenes[i].transform.localPosition = Vector3.SmoothDamp(damageGenes[i].transform.localPosition, targetPos[i + 18], ref velocity, 0.05f);
                damageGenes[i].color = new Color(damageGenes[i].color.r, damageGenes[i].color.g, damageGenes[i].color.b, currentAlpha);
            }
            for (int i = 0; i < 5; ++i)
            {
                Vector3 velocity = Vector3.zero;
                speedGenes[i].transform.localPosition = Vector3.SmoothDamp(speedGenes[i].transform.localPosition, targetPos[i + 23], ref velocity, 0.05f);
                speedGenes[i].color = new Color(speedGenes[i].color.r, speedGenes[i].color.g, speedGenes[i].color.b, currentAlpha);
            }
        }
        else
        {
            currentAlpha = targetAlpha;
        }

        SetHealth();
        if (statsExpanded)
        {
            SetGenes();
        }
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

    void SetGenes()
    {
        //health gene
        if (player.GetGene(0) <= 0.2f)
        {
            for (int i = 1; i < 5; ++i)
            {
                healthGenes[i].enabled = false;
            }
            healthGenes[0].color = new Color(healthGenes[0].color.r, healthGenes[0].color.g, healthGenes[0].color.b, player.GetGene(0) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(0) <= 0.4f)
        {
            for (int i = 2; i < 5; ++i)
            {
                healthGenes[i].enabled = false;
            }
            healthGenes[1].enabled = true;
            healthGenes[0].color = new Color(healthGenes[0].color.r, healthGenes[0].color.g, healthGenes[0].color.b, 1.0f * currentAlpha);
            healthGenes[1].color = new Color(healthGenes[1].color.r, healthGenes[1].color.g, healthGenes[1].color.b, (player.GetGene(0) - 0.2f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(0) <= 0.6f)
        {
            for (int i = 3; i < 5; ++i)
            {
                healthGenes[i].enabled = false;
            }
            for (int i = 0; i < 2; ++i)
            {
                healthGenes[i].enabled = true;
                healthGenes[i].color = new Color(healthGenes[i].color.r, healthGenes[i].color.g, healthGenes[i].color.b, 1.0f * currentAlpha);
            }
            healthGenes[2].color = new Color(healthGenes[2].color.r, healthGenes[2].color.g, healthGenes[2].color.b, (player.GetGene(0) - 0.4f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(0) <= 0.8f)
        {
            for (int i = 4; i < 5; ++i)
            {
                healthGenes[i].enabled = false;
            }
            for (int i = 0; i < 3; ++i)
            {
                healthGenes[i].enabled = true;
                healthGenes[i].color = new Color(healthGenes[i].color.r, healthGenes[i].color.g, healthGenes[i].color.b, 1.0f * currentAlpha);
            }
            healthGenes[3].color = new Color(healthGenes[3].color.r, healthGenes[3].color.g, healthGenes[3].color.b, (player.GetGene(0) - 0.6f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(0) <= 1.0f)
        {
            for (int i = 0; i < 5; ++i)
            {
                healthGenes[i].enabled = true;
                healthGenes[i].color = new Color(healthGenes[i].color.r, healthGenes[i].color.g, healthGenes[i].color.b, 1.0f * currentAlpha);
            }
            healthGenes[4].color = new Color(healthGenes[4].color.r, healthGenes[4].color.g, healthGenes[4].color.b, (player.GetGene(0) - 0.8f) * 5.0f * currentAlpha);
        }

        //range gene
        if (player.GetGene(2) <= 0.2f)
        {
            for (int i = 1; i < 5; ++i)
            {
                rangeGenes[i].enabled = false;
            }
            rangeGenes[0].color = new Color(rangeGenes[0].color.r, rangeGenes[0].color.g, rangeGenes[0].color.b, player.GetGene(1) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(2) <= 0.4f)
        {
            for (int i = 2; i < 5; ++i)
            {
                rangeGenes[i].enabled = false;
            }
            rangeGenes[1].enabled = true;
            rangeGenes[0].color = new Color(rangeGenes[0].color.r, rangeGenes[0].color.g, rangeGenes[0].color.b, 1.0f * currentAlpha);
            rangeGenes[1].color = new Color(rangeGenes[1].color.r, rangeGenes[1].color.g, rangeGenes[1].color.b, (player.GetGene(2) - 0.2f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(2) <= 0.6f)
        {
            for (int i = 3; i < 5; ++i)
            {
                rangeGenes[i].enabled = false;
            }
            for (int i = 0; i < 2; ++i)
            {
                rangeGenes[i].enabled = true;
                rangeGenes[i].color = new Color(rangeGenes[i].color.r, rangeGenes[i].color.g, rangeGenes[i].color.b, 1.0f * currentAlpha);
            }
            rangeGenes[2].color = new Color(rangeGenes[2].color.r, rangeGenes[2].color.g, rangeGenes[2].color.b, (player.GetGene(2) - 0.4f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(2) <= 0.8f)
        {
            for (int i = 4; i < 5; ++i)
            {
                rangeGenes[i].enabled = false;
            }
            for (int i = 0; i < 3; ++i)
            {
                rangeGenes[i].enabled = true;
                rangeGenes[i].color = new Color(rangeGenes[i].color.r, rangeGenes[i].color.g, rangeGenes[i].color.b, 1.0f * currentAlpha);
            }
            rangeGenes[3].color = new Color(rangeGenes[3].color.r, rangeGenes[3].color.g, rangeGenes[3].color.b, (player.GetGene(2) - 0.6f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(2) <= 1.0f)
        {
            for (int i = 0; i < 5; ++i)
            {
                rangeGenes[i].enabled = true;
                rangeGenes[i].color = new Color(rangeGenes[i].color.r, rangeGenes[i].color.g, rangeGenes[i].color.b, 1.0f * currentAlpha);
            }
            rangeGenes[4].color = new Color(rangeGenes[4].color.r, rangeGenes[4].color.g, rangeGenes[4].color.b, (player.GetGene(2) - 0.8f) * 5.0f * currentAlpha);
        }

        //damage gene
        if (player.GetGene(1) <= 0.2f)
        {
            for (int i = 1; i < 5; ++i)
            {
                damageGenes[i].enabled = false;
            }
            damageGenes[0].color = new Color(damageGenes[0].color.r, damageGenes[0].color.g, damageGenes[0].color.b, player.GetGene(1) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(1) <= 0.4f)
        {
            for (int i = 2; i < 5; ++i)
            {
                damageGenes[i].enabled = false;
            }
            damageGenes[1].enabled = true;
            damageGenes[0].color = new Color(damageGenes[0].color.r, damageGenes[0].color.g, damageGenes[0].color.b, 1.0f * currentAlpha);
            damageGenes[1].color = new Color(damageGenes[1].color.r, damageGenes[1].color.g, damageGenes[1].color.b, (player.GetGene(1) - 0.2f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(1) <= 0.6f)
        {
            for (int i = 3; i < 5; ++i)
            {
                damageGenes[i].enabled = false;
            }
            for (int i = 0; i < 2; ++i)
            {
                damageGenes[i].enabled = true;
                damageGenes[i].color = new Color(damageGenes[i].color.r, damageGenes[i].color.g, damageGenes[i].color.b, 1.0f * currentAlpha);
            }
            damageGenes[2].color = new Color(damageGenes[2].color.r, damageGenes[2].color.g, damageGenes[2].color.b, (player.GetGene(1) - 0.4f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(1) <= 0.8f)
        {
            for (int i = 4; i < 5; ++i)
            {
                damageGenes[i].enabled = false;
            }
            for (int i = 0; i < 3; ++i)
            {
                damageGenes[i].enabled = true;
                damageGenes[i].color = new Color(damageGenes[i].color.r, damageGenes[i].color.g, damageGenes[i].color.b, 1.0f * currentAlpha);
            }
            damageGenes[3].color = new Color(damageGenes[3].color.r, damageGenes[3].color.g, damageGenes[3].color.b, (player.GetGene(1) - 0.6f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(1) <= 1.0f)
        {
            for (int i = 0; i < 5; ++i)
            {
                damageGenes[i].enabled = true;
                damageGenes[i].color = new Color(damageGenes[i].color.r, damageGenes[i].color.g, damageGenes[i].color.b, 1.0f * currentAlpha);
            }
            damageGenes[4].color = new Color(damageGenes[4].color.r, damageGenes[4].color.g, damageGenes[4].color.b, (player.GetGene(1) - 0.8f) * 5.0f * currentAlpha);
        }

        //speed gene
        if (player.GetGene(3) <= 0.2f)
        {
            for (int i = 1; i < 5; ++i)
            {
                speedGenes[i].enabled = false;
            }
            speedGenes[0].color = new Color(speedGenes[0].color.r, speedGenes[0].color.g, speedGenes[0].color.b, player.GetGene(3) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(3) <= 0.4f)
        {
            for (int i = 2; i < 5; ++i)
            {
                speedGenes[i].enabled = false;
            }
            speedGenes[1].enabled = true;
            speedGenes[0].color = new Color(speedGenes[0].color.r, speedGenes[0].color.g, speedGenes[0].color.b, 1.0f * currentAlpha);
            speedGenes[1].color = new Color(speedGenes[1].color.r, speedGenes[1].color.g, speedGenes[1].color.b, (player.GetGene(3) - 0.2f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(3) <= 0.6f)
        {
            for (int i = 3; i < 5; ++i)
            {
                speedGenes[i].enabled = false;
            }
            for (int i = 0; i < 2; ++i)
            {
                speedGenes[i].enabled = true;
                speedGenes[i].color = new Color(speedGenes[i].color.r, speedGenes[i].color.g, speedGenes[i].color.b, 1.0f * currentAlpha);
            }
            speedGenes[2].color = new Color(speedGenes[2].color.r, speedGenes[2].color.g, speedGenes[2].color.b, (player.GetGene(3) - 0.4f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(3) <= 0.8f)
        {
            for (int i = 4; i < 5; ++i)
            {
                speedGenes[i].enabled = false;
            }
            for (int i = 0; i < 3; ++i)
            {
                speedGenes[i].enabled = true;
                speedGenes[i].color = new Color(speedGenes[i].color.r, speedGenes[i].color.g, speedGenes[i].color.b, 1.0f * currentAlpha);
            }
            speedGenes[3].color = new Color(speedGenes[3].color.r, speedGenes[3].color.g, speedGenes[3].color.b, (player.GetGene(3) - 0.6f) * 5.0f * currentAlpha);
        }
        else if (player.GetGene(3) <= 1.0f)
        {
            for (int i = 0; i < 5; ++i)
            {
                speedGenes[i].enabled = true;
                speedGenes[i].color = new Color(speedGenes[i].color.r, speedGenes[i].color.g, speedGenes[i].color.b, 1.0f * currentAlpha);
            }
            speedGenes[4].color = new Color(speedGenes[4].color.r, speedGenes[4].color.g, speedGenes[4].color.b, (player.GetGene(3) - 0.8f) * 5.0f * currentAlpha);
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
            for (int i = 8; i < 13; ++i)
            {
                targetPos[i] = new Vector3(-150.0f, 150.0f, 0);
            }
            for (int i = 13; i < 18; ++i)
            {
                targetPos[i] = new Vector3(-150.0f, -150.0f, 0);
            }
            for (int i = 18; i < 23; ++i)
            {
                targetPos[i] = new Vector3(150.0f, -150.0f, 0);
            }
            for (int i = 23; i < 28; ++i)
            {
                targetPos[i] = new Vector3(150.0f, 150.0f, 0);
            }

            targetAlpha = 0.5f;
        }
        else
        {
            for (int i = 0; i < 8; ++i)
            {
                targetPos[i] = healthImages[i].transform.localPosition / 2;
            }
            for (int i = 8; i < 28; ++i)
            {
                targetPos[i] = new Vector3(0, 0, 0);
            }

            targetAlpha = 0.0f;
        }

        expanding = 1.0f;
    }
}
