using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour {

    public float radius;
    public float spawnDelay;
    public float speed;
    public float upperLimit;

    private float spawnCooldown;

    public GameObject obstacle;

    private List<GameObject> objects;
    private Vector3 velocity;

    void Awake()
    {
        spawnCooldown = spawnDelay;

        objects = new List<GameObject>();

        velocity = new Vector3(0, speed, 0);
    }

	void Start () 
    {
        //spawn all initial obstacles
        for (float i = transform.position.y; i < upperLimit; i += speed * spawnDelay)
        {
            Vector3 randomPos = Random.insideUnitSphere * radius;
            randomPos.y = i;
            GameObject newObject = (GameObject)Instantiate(obstacle, randomPos, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
            objects.Add(newObject);
        }
	}
	
	void Update () 
    {
        spawnCooldown -= Time.deltaTime;

        //update the current obstacles
	    for (int i = objects.Count - 1; i >= 0; --i)
        {
            objects[i].transform.position += velocity * Time.deltaTime;

            if (objects[i].transform.position.y > upperLimit)
            {
                Destroy(objects[i]);
                objects.RemoveAt(i);
            }
        }

        //spawn new obstacles
        if (spawnCooldown <= 0)
        {
            Vector3 randomPos = Random.insideUnitSphere * radius;
            randomPos.y = transform.position.y;
            GameObject newObject = (GameObject)Instantiate(obstacle, randomPos, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
            objects.Add(newObject);
        }

        if (spawnCooldown <= 0)
            spawnCooldown = spawnDelay;
	}
}
