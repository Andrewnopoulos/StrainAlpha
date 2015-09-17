using UnityEngine;
using System.Collections;

public class CampScript : MonoBehaviour {

    public int NeutralType;
    private float SpawnDistanceScale;

    public GameObject CellPrefab;

	// Use this for initialization
	void Start () {
	    
	}

    void Awake()
    {
        SpawnDistanceScale = transform.localScale.x;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
