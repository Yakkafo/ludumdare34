using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

    public List<Spot> towerSpots;
    public const int MAX_HEIGHT = 8;

	// Use this for initialization
	void Start () {
        towerSpots = new List<Spot>();

        for (int i = 0; i < MAX_HEIGHT * 4; i++) {
            Spot spot = new Spot(i);
            towerSpots.Add(spot);
        }
        for(int i = 0; i < 8; i++)
        {
            Debug.Log(towerSpots[i].gameplayPosition + " has for neighbourg " + IDToGameplayPosition(NextNeighbourID(towerSpots[i])));
        }
        
    }

    // Update is called once per frame
    void Update () {
	
	}

    public int NextNeighbourID(int _currentID) {
        return NextNeighbourID(towerSpots[_currentID]);
    }

    public int NextNeighbourID(Spot _currentSpot) {
        Vector3 nextGameplayPosition = _currentSpot.gameplayPosition;
        if (nextGameplayPosition.y == 1) {
            switch ((int) nextGameplayPosition.x) {
                case 0:
                    nextGameplayPosition.x = nextGameplayPosition.y = 0;
                    nextGameplayPosition.z++;
                    break;
                case 1:
                    nextGameplayPosition.x--;
                    break;
            }
        }
        else {
            switch ((int)nextGameplayPosition.x)
            {
                case 0:
                    nextGameplayPosition.x ++;
                    break;
                case 1:
                    nextGameplayPosition.y ++;
                    break;
            }
        }

        return GameplayPositionToID(nextGameplayPosition);
    }

    public int GameplayPositionToID(Vector3 _gameplayPosition) {
        foreach(Spot spot in towerSpots) {
            if (spot.gameplayPosition == _gameplayPosition)
                return spot.id;
        }
        return -1;
    }

    public Vector3 IDToGameplayPosition(int _id) {
        return towerSpots[_id].gameplayPosition;
    }
}
