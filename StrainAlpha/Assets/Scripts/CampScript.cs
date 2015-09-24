using UnityEngine;
using System.Collections;

public class CampScript : MonoBehaviour {

    public GameObject CellPrefab;

    public NPCManager manager;

    private SphereCollider spawnCollider;

    public int NeutralType;
    public int population;
    private float SpawnDistanceScale;

    public bool canSpawn;

    public float spawnTimer;

	// Use this for initialization
	void Start () {
        manager = FindObjectOfType<NPCManager>();

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        SpawnDistanceScale = transform.localScale.x / 2;

        for (int i = 0; i < population; i++)
        {
            SpawnNeutralCell();
        }

        canSpawn = true;
        spawnTimer = 5.0f;
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
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        
        if (canSpawn && spawnTimer < 0)
        {
            SpawnNeutralCell();
            spawnTimer = manager.SpawnUrgency;
        }
    }
}
