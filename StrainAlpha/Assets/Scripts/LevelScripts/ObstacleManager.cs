using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObstacleType
{
    BOTTOM_UP,
    RANDOM_MOVEMENT,
}

public class ObstacleManager : MonoBehaviour {

    public ObstacleType type = ObstacleType.BOTTOM_UP;

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
        if (type == ObstacleType.BOTTOM_UP)
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
        else
        {
            for (int i = 0; i < upperLimit; i++)
            {
                Vector3 randomPos = Random.insideUnitSphere * radius;
                randomPos.y = 0;
                GameObject newObject = (GameObject)Instantiate(obstacle, randomPos, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
                objects.Add(newObject);
            }
        }
	}
	
	void Update () 
    {
        spawnCooldown -= Time.deltaTime;

        if (type == ObstacleType.BOTTOM_UP)
        {
            //update the current obstacles
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                objects[i].transform.position += velocity * Time.deltaTime;
                objects[i].transform.Rotate(velocity * Time.deltaTime);
                if (objects[i].transform.position.y > upperLimit)
                {
                    Destroy(objects[i]);
                    objects.RemoveAt(i);
                }
            }
        }
        else
        {
            //update the current obstacles
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                //objects[i].transform.position += velocity * Time.deltaTime;
                objects[i].transform.Rotate(velocity * Time.deltaTime);
                objects[i].transform.position = new Vector3(objects[i].transform.position.x, 0, objects[i].transform.position.z);
            }
        }

        if (type == ObstacleType.BOTTOM_UP)
        {
            //spawn new obstacles
            if (spawnCooldown <= 0)
            {
                Vector3 randomPos = Random.insideUnitSphere * radius;
                randomPos.y = transform.position.y;
                GameObject newObject = (GameObject)Instantiate(obstacle, randomPos, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
                objects.Add(newObject);
            }
        }

        if (spawnCooldown <= 0)
            spawnCooldown = spawnDelay;
	}
}
