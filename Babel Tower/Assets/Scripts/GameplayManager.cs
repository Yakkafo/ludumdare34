using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour {

    public Tower tower;
    public GameplayCamera gameplayCamera;
    private int currentSpotID = 0;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space)) {
            if (gameplayCamera.currentState != GameplayCamera.CameraState.MoveTo) {
                tower.CreateNextBlockAtID(currentSpotID);
                currentSpotID = tower.NextNeighbourID(currentSpotID);
                gameplayCamera.target.y = tower.IDToGameplayPosition(currentSpotID).y + gameplayCamera.initialHeigh;
                gameplayCamera.MoveToNextTarget();
            }
        }
	}
    
}
