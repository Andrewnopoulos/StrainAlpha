using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour {

    public GameObject cell;

    public GameObject nucleus;

    private List<CellScript> friendlyList;
    private List<CellScript> infectedList;
    private List<CellScript> killList;

    public int InitialNeutralCells = 40;

    public int InitialInfectedCells = 2;

    void Start()
    {
        for (int i = 0; i < InitialNeutralCells; i++)
        {
            SpawnNeutral();
        }

        for (int i = 0; i < InitialInfectedCells; i++)
        {
           CreateInfectedCell(new Chromosome(0.05f), new Vector3(10, 0, 10));
        }
    }

    void SpawnNeutral()
    {
        Vector3 randomPos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));

        GameObject newCell = (GameObject)Instantiate(cell, randomPos, transform.rotation);
        CellScript script = newCell.GetComponent<CellScript>();
        script.manager = this;

        friendlyList.Add(script);
    }

    void Awake()
    {
        friendlyList = new List<CellScript>();
        infectedList = new List<CellScript>();
        killList = new List<CellScript>();
    }

    void Update()
    {
        UpdateInfectedTargets();

        UpdateDeadInfected();
    }

    void UpdateInfectedTargets()
    {
        foreach (CellScript enemy in infectedList)
        {

        }
    }

    void SpawnNucleus(CellScript inputCell)
    {
        GameObject newNucleus = (GameObject)Instantiate(nucleus, inputCell.transform.position, inputCell.transform.rotation);
        NucleusScript script = newNucleus.GetComponent<NucleusScript>();
        script.SetChromosome(inputCell.GetChromosome());
        script.SetVelocity(inputCell.velocity);
    }

    public void AddToKillList(CellScript npc)
    {
        killList.Add(npc);
        infectedList.Remove(npc);
    }

    private void UpdateDeadInfected()
    {
        for (int i = killList.Count - 1; i >= 0; --i)
        {
            SpawnNucleus(killList[i]);
            Destroy(killList[i].gameObject);
            killList.Remove(killList[i]);
        }
    }

    private void CreateInfectedCell(Chromosome _inputChromosome, Vector3 _location)
    {
        GameObject newCell = (GameObject)Instantiate(cell, _location, transform.rotation);
        CellScript script = newCell.GetComponent<CellScript>();
        script.manager = this;
        script.BecomeInfected(_inputChromosome);

        infectedList.Add(script);
    }

    private void InfectNeutralCell(CellScript _neutralCell, Chromosome _inputChromosome)
    {
        _neutralCell.BecomeInfected(_inputChromosome);

        friendlyList.Remove(_neutralCell);
        infectedList.Add(_neutralCell);
    }

    //private float spawnRate = 1.0f;
    //private float spawnCooldown = 0.0f;

    //// Use this for initialization
    //void Start () {
	
    //    npcList = new List<GameObject>();
    //    killList = new List<GameObject>();

    //}
	
    //// Update is called once per frame
    //void Update () {

    //    if (spawnCooldown > 0)
    //        spawnCooldown -= Time.deltaTime;
    //    else
    //    {
    //        spawnCooldown = spawnRate;

    //        Vector3 randomPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

    //        GameObject newCell = (GameObject)Instantiate(cell, randomPos, transform.rotation);
    //        newCell.GetComponent<CellScript>().manager = this;
    //        AddNPC(newCell);
    //    }



    //}

    //void CheckInfections()
    //{
    //    for (int i = 0; i < )
    //}


    //void CreateNPC()
    //{

    //}

    //public void AddNPC(GameObject npc)
    //{
    //    npcList.Add(npc);
    //}


}
