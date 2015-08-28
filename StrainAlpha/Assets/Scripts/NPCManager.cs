using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour {

    public GameObject cell;

    public GameObject nucleus;

    private List<GameObject> npcList;
    private List<GameObject> killList;

    private float spawnRate = 1.0f;
    private float spawnCooldown = 0.0f;

	// Use this for initialization
	void Start () {
	
        npcList = new List<GameObject>();
        killList = new List<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {

        if (spawnCooldown > 0)
            spawnCooldown -= Time.deltaTime;
        else
        {
            spawnCooldown = spawnRate;

            Vector3 randomPos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

            GameObject newCell = (GameObject)Instantiate(cell, randomPos, transform.rotation);
            newCell.GetComponent<CellScript>().manager = this;
            AddNPC(newCell);
        }

        for (int i = killList.Count - 1; i >= 0; --i)
        {
            SpawnNucleus(killList[i].GetComponent<CellScript>());
            Destroy(killList[i]);
            killList.Remove(killList[i]);
        }

	}

    void SpawnNucleus(CellScript inputCell)
    {
        GameObject newNucleus = (GameObject)Instantiate(nucleus, inputCell.transform.position, inputCell.transform.rotation);
        newNucleus.GetComponent<NucleusScript>().SetChromosome(inputCell.GetChromosome());
    }

    void CreateNPC()
    {

    }

    public void AddNPC(GameObject npc)
    {
        npcList.Add(npc);
    }

    public void AddToKillList(GameObject npc)
    {
        killList.Add(npc);
        npcList.Remove(npc);
    }
}
