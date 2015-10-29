using UnityEngine;
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

    private Image selectedImage;
    private Image healthGene;
    private Image rangeGene;
    private Image damageGene;
    private Image speedGene;

    private Image threatBar;
    private float threat;

    private int score = 0;
    private Text scoreText;
    private float baseScoreSize = 0.4f;
    private float scoreSize = 0.4f;

    private Text timeTextMinute;
    private Text timeTextSecond;
    private Text timeTextMilli;
    private float time = 0.0f;

    private float previousHealth;
    private float previousRange;
    private float previousDamage;
    private float previousSpeed;

    private PlayerScript player;

    private GameObject anchor;

    private Camera mainCamera;

	// Use this for initialization
	void Start () {

        //allImages = new Image[17];

        healthImages = new Image[16];
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

        selectedImage = allImages[8];
        healthGene = allImages[10];
        rangeGene = allImages[12];
        damageGene = allImages[14];
        speedGene = allImages[16];

        healthImages[8] = allImages[17];
        healthImages[9] = allImages[18];
        healthImages[10] = allImages[19];
        healthImages[11] = allImages[20];
        healthImages[12] = allImages[21];
        healthImages[13] = allImages[22];
        healthImages[14] = allImages[23];
        healthImages[15] = allImages[24];

        threatBar = allImages[26];
        threat = 0.0f;

        scoreText = gameObject.GetComponentsInChildren<Text>()[0];
        timeTextMinute = gameObject.GetComponentsInChildren<Text>()[1];
        timeTextSecond = gameObject.GetComponentsInChildren<Text>()[2];
        timeTextMilli = gameObject.GetComponentsInChildren<Text>()[3];

        player = GameObject.Find("Player").GetComponent<PlayerScript>();

        anchor = GameObject.Find("PlayerAnchor");

        mainCamera = Camera.allCameras[0];

        SetGenes();
	}
	
	// Update is called once per frame
	void Update () {

        //cast heaven
        timeTextMinute.text = ((int)(time / 60.0f)).ToString();
        timeTextSecond.text = ((int)(time - (60.0f * (float)((int)(time / 60.0f))))).ToString();
        timeTextMilli.text = ((int)(100 * ((time - ((60.0f * (float)((int)(time / 60.0f))))) - ((float)((int)(time - (60.0f * (float)((int)(time / 60.0f))))))))).ToString();

        NeatenScore();

        threatBar.transform.localScale = new Vector3(threat, 1, 1);

        scoreText.text = score.ToString();

        scoreSize -= (scoreSize - baseScoreSize) * Time.deltaTime * 2.0f;

        scoreText.transform.localScale = new Vector3(scoreSize, scoreSize, scoreSize);

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
                healthImages[i + 8].transform.localPosition = healthImages[i].transform.localPosition;
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

    void NeatenScore()
    {
        if (timeTextSecond.text.Length == 1)
        {
            timeTextSecond.text = "0" + timeTextSecond.text;
        }
        if (timeTextMilli.text.Length == 1)
        {
            timeTextMilli.text = "0" + timeTextMilli.text;
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
        healthGene.transform.RotateAround(anchor.transform.position, transform.forward, (previousHealth - player.GetGene(0)) * 90);
        rangeGene.transform.RotateAround(anchor.transform.position, transform.forward, (player.GetGene(2) - previousRange) * 90);
        damageGene.transform.RotateAround(anchor.transform.position, transform.forward, (previousDamage - player.GetGene(1)) * 90);
        speedGene.transform.RotateAround(anchor.transform.position, transform.forward, (player.GetGene(3) - previousSpeed) * 90);

        previousHealth = player.GetGene(0);
        previousRange = player.GetGene(2);
        previousDamage = player.GetGene(1);
        previousSpeed = player.GetGene(3);
       
    }

    public void RotateRight()
    {
        selectedImage.transform.RotateAround(anchor.transform.position, transform.forward, 90);
    }

    public void RotateLeft()
    {
        selectedImage.transform.RotateAround(anchor.transform.position, transform.forward, -90);
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

    public void AddScore(int _score)
    {
        score += _score;
        scoreSize = 0.6f;
    }
    public void SetTime(float _time)
    {
        time = _time;
    }

    public void SetThreat(float _threat)
    {
        threat = _threat;
    }
}
