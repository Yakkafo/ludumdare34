using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

    public List<Spot> towerSpots;
    public const int MAX_HEIGHT = 8;

	// Use this for initialization
	void Start () {
	    for(int i = 0; i < MAX_HEIGHT; i++) {
            Spot spot = new Spot();
            towerSpots.Add(spot);

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
