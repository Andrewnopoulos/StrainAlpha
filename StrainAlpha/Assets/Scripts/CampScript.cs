using UnityEngine;
using System.Collections;

public class CampScript : MonoBehaviour {

    public GameObject CellPrefab;

    public NPCManager manager;

    public int NeutralType;
    public int population;
    private float SpawnDistanceScale;

	// Use this for initialization
	void Start () {
        manager = FindObjectOfType<NPCManager>();

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        SpawnDistanceScale = transform.localScale.x / 2;

        for (int i = 0; i < population; i++)
        {
            SpawnNeutralCell();
        }
	}

    void Awake()
    {
        
    }

    void SpawnNeutralCell()
    {
        Chromosome outputChromosome = new Chromosome(NeutralType);
        Vector2 RandomPosition = Random.insideUnitCircle * SpawnDistanceScale;
        Vector3 spawnLocation = new Vector3(transform.position.x + RandomPosition.x, 0, transform.position.z + RandomPosition.y);
        manager.SpawnNeutral(spawnLocation, outputChromosome);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
