using UnityEngine;
using System.Collections;

public class BlendColourScript : MonoBehaviour {

    public bool infected;

    private Renderer myRenderer;

    private float colourBlend = 0.0f;

    private InfectedSpecialType type;

    public float colorBrightness = 0.5f;
    public float enemyOpacity = 0.5f;

	// Use this for initialization
	void Start () {
        infected = false;
        myRenderer = GetComponent<Renderer>();
        colourBlend = 0.0f;
        type = GetComponentInParent<CellScript>().infectedType;
	}
	
	// Update is called once per frame
	void Update () {
	    if (infected)
        {
            if (colourBlend < colorBrightness)
            {
                colourBlend += Time.deltaTime;
            }

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
}
