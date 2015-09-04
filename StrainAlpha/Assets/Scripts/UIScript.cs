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

	// Use this for initialization
	void Start () {

        playerScript = player.GetComponent<PlayerScript>();

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

        healthGene.rectTransform.localPosition = new Vector3(-570.0f, 25.0f + (125.0f * playerScript.GetGene(0)));
        damageGene.rectTransform.localPosition = new Vector3(570.0f, 25.0f + (125.0f * playerScript.GetGene(1)));
        rangeGene.rectTransform.localPosition = new Vector3(-570.0f, -25.0f - (125.0f * playerScript.GetGene(2)));
        speedGene.rectTransform.localPosition = new Vector3(570.0f, -25.0f - (125.0f * playerScript.GetGene(3)));

	}
}
