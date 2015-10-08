using UnityEngine;
using System.Collections;

public class BlendColourScript : MonoBehaviour {

    public bool infected;

    private Renderer myRenderer;

    private float colourBlend = 0.0f;

    private InfectedSpecialType type;

    public bool blending = true;

    public float colorBrightness = 0.3f;
    public float enemyOpacity = 0.5f;

    private CellScript parentScript;

    private Chromosome cellGenes;

	// Use this for initialization
	void Start () {
        infected = false;
        myRenderer = GetComponent<Renderer>();
        colourBlend = 0.0f;
        parentScript = GetComponentInParent<CellScript>();
        type = parentScript.infectedType;
        cellGenes = parentScript.GetChromosome();
	}
	
	// Update is called once per frame
	void Update () {
	    if (infected)
        {
            if (colourBlend < colorBrightness)
            {
                colourBlend += Time.deltaTime;
            }

            cellGenes = parentScript.GetChromosome();

            if (blending)
            {
                BlendColours();
            }
            else
            {
                DiscreteColours();
            }
        }
	}

    void BlendColours()
    {
        float redvalue = cellGenes[1];
        float greenvalue = cellGenes[0];
        float bluevalue = cellGenes[2];
        float yellowvalue = cellGenes[3];

        Vector3 colour = new Vector3(redvalue, greenvalue, bluevalue);

        colour.Normalize();

        colour *= colourBlend;

        myRenderer.material.color = new Color(colour.x, colour.y, colour.z);
    }

    void DiscreteColours()
    {
        switch (type)
        {
            case InfectedSpecialType.HEALTH:
                myRenderer.material.color = new Color(0, colourBlend, 0, enemyOpacity);
                break;
            case InfectedSpecialType.SPEED:
                myRenderer.material.color = new Color(colourBlend / 2, colourBlend / 2, 0, enemyOpacity);
                break;
            case InfectedSpecialType.RANGED:
                myRenderer.material.color = new Color(0, 0, colourBlend, enemyOpacity);
                break;
            case InfectedSpecialType.REPLICATION:
                myRenderer.material.color = new Color(0, colourBlend / 2, colourBlend / 2, enemyOpacity);
                break;
            case InfectedSpecialType.KAMIKAZE:
                myRenderer.material.color = new Color(colourBlend / 2, 0, colourBlend / 2, enemyOpacity);
                break;

            default:
                myRenderer.material.color = new Color(colourBlend, 0, 0, enemyOpacity);
                break;
        }
    }
}
