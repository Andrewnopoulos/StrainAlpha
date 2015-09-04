using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScript : MonoBehaviour {

    public GameObject player;
    private PlayerScript playerScript;

    private Image healthGene;
    private Image damageGene;
    private Image rangeGene;
    private Image speedGene;

    private Image[] backgrounds;

	// Use this for initialization
	void Start () {

        playerScript = player.GetComponent<PlayerScript>();

        backgrounds = new Image[4];

        backgrounds[0] = gameObject.GetComponentsInChildren<Image>()[0];
        backgrounds[1] = gameObject.GetComponentsInChildren<Image>()[1];
        backgrounds[2] = gameObject.GetComponentsInChildren<Image>()[2];
        backgrounds[3] = gameObject.GetComponentsInChildren<Image>()[3];

        healthGene = gameObject.GetComponentsInChildren<Image>()[4];
        damageGene = gameObject.GetComponentsInChildren<Image>()[5];
        rangeGene = gameObject.GetComponentsInChildren<Image>()[6];
        speedGene = gameObject.GetComponentsInChildren<Image>()[7];

	}
	
	// Update is called once per frame
	void Update () {

        healthGene.rectTransform.localScale = new Vector3(1, playerScript.GetGene(0), 1);
        damageGene.rectTransform.localScale = new Vector3(1, playerScript.GetGene(1), 1);
        rangeGene.rectTransform.localScale = new Vector3(1, playerScript.GetGene(2), 1);
        speedGene.rectTransform.localScale = new Vector3(1, playerScript.GetGene(3), 1);

        healthGene.rectTransform.localPosition = new Vector3(-(Screen.width / 2 * 0.93f), Screen.height / 2 * 0.06f + (125.0f * playerScript.GetGene(0)));
        damageGene.rectTransform.localPosition = new Vector3((Screen.width / 2 * 0.93f), Screen.height / 2 * 0.06f + (125.0f * playerScript.GetGene(1)));
        rangeGene.rectTransform.localPosition = new Vector3(-(Screen.width / 2 * 0.93f), -(Screen.height / 2 * 0.06f + (125.0f * playerScript.GetGene(2))));
        speedGene.rectTransform.localPosition = new Vector3((Screen.width / 2 * 0.93f), -(Screen.height / 2 * 0.06f + (125.0f * playerScript.GetGene(3))));

        backgrounds[0].rectTransform.localPosition = new Vector3(-(Screen.width / 2 * 0.93f), Screen.height / 2 * 0.5f);
        backgrounds[1].rectTransform.localPosition = new Vector3((Screen.width / 2 * 0.93f), Screen.height / 2 * 0.5f);
        backgrounds[2].rectTransform.localPosition = new Vector3(-(Screen.width / 2 * 0.93f), -(Screen.height / 2 * 0.5f));
        backgrounds[3].rectTransform.localPosition = new Vector3((Screen.width / 2 * 0.93f), -(Screen.height / 2 * 0.5f));  

	}
}
