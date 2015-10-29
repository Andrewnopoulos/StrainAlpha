using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour {

    public float levelTimer;

    private float preGameCounter;

    public GameObject cell;

    public GameObject nucleus;

    public GameObject explosion;

    public GameObject boss;

    private PlayerScript playerScript;

    private List<CellScript> friendlyList;
    private List<CellScript> infectedList;
    private List<CellScript> killList;

    private BossScript endBoss;

    private PlayerUI ui;
    private CountDown countdownUI;

    private CameraFollow cameraScript;

    public int InitialNeutralCells = 40;

    public int InitialInfectedCells = 10;

    public float infectionRange = 1.0f;

    public float SpawnUrgency = 0.0f;

    public int LargeNumberOfCells = 600;

    public float SpawnUrgencyScale = 5.0f;

    public int bossSpawnKillCount;

    public int currentKills;

    private bool bossSpawn = false;

    private float bossBirthTime;

    private float endLevelTime = 3.0f;

    private GameObject syringe;

    private DifficultySelection difficulty;

    void Start()
    {
        for (int i = 0; i < InitialNeutralCells; i++)
        {
            SpawnNeutral();
        }

        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();

        ui = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        countdownUI = GameObject.Find("countdownCanvas").GetComponent<CountDown>();

        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

        SpawnUrgency = 5.0f;

        syringe = GameObject.Find("syringe");

        ui.gameObject.SetActive(false);

        preGameCounter = 4.0f;

        difficulty = DifficultySelection.EASY;

        if (GameObject.Find("PersistentObject(Clone)"))
        {
            difficulty = GameObject.Find("PersistentObject(Clone)").GetComponent<PersistentData>().difficulty;
            Destroy(GameObject.Find("PersistentObject(Clone)"));
        }

        if (difficulty == DifficultySelection.EASY)
        {
            levelTimer = 300.0f;
        }
        else if (difficulty == DifficultySelection.MEDIUM)
        {
            levelTimer = 240.0f;
        }
        else
        {
            levelTimer = 180.0f;
        }
    }

    void Update()
    {
        if (preGameCounter > 0)
        {
            preGameCounter -= Time.deltaTime;
            if (preGameCounter > 3.0f)
            {
                if (countdownUI.GetCountdown() != "")
                    countdownUI.SetCountdown("");
            }
            else if (preGameCounter > 2.0f)
            {
                if (countdownUI.GetCountdown() != "3")
                    countdownUI.SetCountdown("3");
            }
            else if (preGameCounter > 1.0f)
            {
                if (countdownUI.GetCountdown() != "2")
                    countdownUI.SetCountdown("2");
            }
            else if (preGameCounter > 0.0f)
            {
                if (countdownUI.GetCountdown() != "1")
                    countdownUI.SetCountdown("1");
            }

            return;

        }
        else if (preGameCounter < 0)
        {
            countdownUI.SetCountdown("");

            preGameCounter = 0;

            //spawn enemy cells now
            for (int i = 0; i < InitialInfectedCells; i++)
            {
                Vector2 randomSpawn = Random.insideUnitCircle * 50;
                CreateInfectedCell(new Chromosome(4), new Vector3(randomSpawn.x, 0, randomSpawn.y));
            }

            //enable player movement
            playerScript.spawning = false;

            //change camera position
            cameraScript.SetPlayerDist(1);
            cameraScript.smoothTime = 0.05f;

            //hide the syringe
            syringe.SetActive(false);

            ui.gameObject.SetActive(true);

            return;
        }

        if (levelTimer > 0)
            levelTimer -= Time.deltaTime;
        else
            EndLevel();
        ui.SetTime(levelTimer);

        int CellCount = friendlyList.Count + infectedList.Count;

        SpawnUrgency = (float)CellCount / LargeNumberOfCells * SpawnUrgencyScale;

        if (currentKills >= bossSpawnKillCount && !bossSpawn)
        {
            SpawnBoss();
        }

        if (bossBirthTime > 0)
            bossBirthTime -= Time.deltaTime;

        if (bossBirthTime <= 0 && bossBirthTime > -100 && bossSpawn)
        {
            //stop enemies from being attracted
            foreach (CellScript cell in infectedList)
            {
                cell.beAbsorbed = false;
            }

            bossBirthTime = -101.0f;
        }

        if (bossSpawn && endBoss == null)
        {
            endLevelTime -= Time.deltaTime;
            if (endLevelTime <= 0)
                EndLevel();
        }
    }

    void EndLevel()
    {
        //clean up after this scene
        //switch to the next scene
        Application.LoadLevel("MainMenu");
    }

    void SpawnNeutral()
    {
        Vector3 randomPos = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20));

        GameObject newCell = (GameObject)Instantiate(cell, randomPos, transform.rotation);
        CellScript script = newCell.GetComponent<CellScript>();

        script.manager = this;

        friendlyList.Add(script);
    }

    public void SpawnNeutral(Vector3 position, Chromosome inputChromosome)
    {
        GameObject newCell = (GameObject)Instantiate(cell, position, transform.rotation);
        CellScript script = newCell.GetComponent<CellScript>();

        script.manager = this;

        script.SetChromosome(inputChromosome);

        friendlyList.Add(script);
    }

    public void SpawnBoss()
    {
        GameObject bossy = (GameObject)Instantiate(boss, new Vector3(0, 0, 0), transform.rotation);
        endBoss = bossy.GetComponent<BossScript>();
        endBoss.manager = this;
        bossSpawn = true;

        bossBirthTime = endBoss.birthTime;

        foreach (CellScript cell in infectedList)
        {
            cell.targetLocation = bossy.transform;
            cell.beAbsorbed = true;
        }

        cameraScript.SetPlayerDist(2);

    }

    void Awake()
    {
        friendlyList = new List<CellScript>();
        infectedList = new List<CellScript>();
        killList = new List<CellScript>();
    }

    void FixedUpdate()
    {

        UpdateDeadInfected();
    }

    public List<CellScript> GetNeutralCells()
    {
        return friendlyList;
    }

    void SpawnNucleus(CellScript inputCell)
    {
        GameObject newNucleus = (GameObject)Instantiate(nucleus, inputCell.transform.position, inputCell.transform.rotation);
        NucleusScript script = newNucleus.GetComponent<NucleusScript>();
        script.SetChromosome(inputCell.GetChromosome());
        script.SetVelocity(inputCell.velocity);
        script.attachedToCell = false;
    }

    public void AddToKillList(CellScript npc)
    {
        infectedList.Remove(npc);
        killList.Add(npc);
    }

    private void UpdateDeadInfected()
    {
        for (int i = killList.Count - 1; i >= 0; --i)
        {
            
            if (killList[i].infectedType == InfectedSpecialType.KAMIKAZE)
            {
                GameObject newExplosion = (GameObject)Instantiate(explosion, killList[i].transform.position, killList[i].transform.rotation);
            }
            else if (killList[i].infectedType == InfectedSpecialType.MINE)
            {
                GameObject newExplosion = (GameObject)Instantiate(explosion, killList[i].transform.position, killList[i].transform.rotation);
                newExplosion.transform.localScale *= 3.5f;
            }
            else
            {
                SpawnNucleus(killList[i]);
            }

            ui.AddScore(killList[i].scoreWorth);
            currentKills++;

            Destroy(killList[i].gameObject);
            killList.Remove(killList[i]);
        }
    }

    public void CreateInfectedCell(Chromosome _inputChromosome, Vector3 _location)
    {
        GameObject newCell = (GameObject)Instantiate(cell, _location, transform.rotation);
        CellScript script = newCell.GetComponent<CellScript>();
        script.manager = this;
        script.CreateInfected(_inputChromosome);

        infectedList.Add(script);
    }

    public void InfectNeutralCell(CellScript _neutralCell)
    {
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
