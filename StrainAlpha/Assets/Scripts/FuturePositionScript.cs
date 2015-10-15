using UnityEngine;
using System.Collections;

public class FuturePositionScript : MonoBehaviour {

    public PlayerScript player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.PlayerFuturePosition();
	}
}
