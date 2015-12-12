using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour {

    public List<Spot> towerSpots;
    public const int MAX_HEIGHT = 8;
    public Block blockModel;

	// Use this for initialization
	void Start () {
        towerSpots = new List<Spot>();

        for (int i = 0; i < MAX_HEIGHT * 4; i++) {
            Spot spot = new Spot(i);
            towerSpots.Add(spot);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void CreateNextBlockAtID(int _id) {
        if (blockModel != null)
            Instantiate(blockModel, IDToGameplayPosition(_id), Quaternion.identity);
    }

    // Trouve l'id du prochain spot suivant l'ordre logique (qu'il soit vide ou non)
    public int NextNeighbourID(int _currentID)
    {
        return NextNeighbourID(towerSpots[_currentID]);
    }

    public int NextNeighbourID(Spot _currentSpot) {
        Vector3 nextGameplayPosition = _currentSpot.gameplayPosition;
        if (nextGameplayPosition.z == 1) {
            switch ((int) nextGameplayPosition.x) {
                case 0:
                    nextGameplayPosition.x = nextGameplayPosition.z = 0;
                    nextGameplayPosition.y++;
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
                    nextGameplayPosition.z ++;
                    break;
            }
        }

        return GameplayPositionToID(nextGameplayPosition);
    }

    // Renvoie l'id d'un spot suivant sa gameplayPosition
    public int GameplayPositionToID(Vector3 _gameplayPosition) {
        foreach(Spot spot in towerSpots) {
            if (spot.gameplayPosition == _gameplayPosition)
                return spot.id;
        }
        return -1;
    }

    // Renvoie la gameplayPosition d'un spot suivant son id
    public Vector3 IDToGameplayPosition(int _id) {
        return towerSpots[_id].gameplayPosition;
    }
    
}
