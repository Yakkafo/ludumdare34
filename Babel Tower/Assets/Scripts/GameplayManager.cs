using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    public Tower tower;
    private int currentSpotID = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space)) {
            tower.CreateNextBlockAtID(currentSpotID);
            currentSpotID = tower.NextNeighbourID(currentSpotID);
        }
	}
}
