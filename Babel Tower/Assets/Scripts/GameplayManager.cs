using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {

    public enum GameplayState {
        Zero = 0,
        BlockSelection,
        TurnTransition,
        NumberOfStates
    }

    private GameplayState currentGameplaystate = GameplayState.BlockSelection;
    public Tower tower;
    public GameplayCamera gameplayCamera;
    private int currentSpotID = 0;

    // Choisir un block
    private List<Block.BlockType> proposedBlocks;
    private int selectedBlock = 0;
    private const int MAX_PROPOSED_BLOCKS = 3;

	void Start () {
        proposedBlocks = new List<Block.BlockType>();
        ResetHand();
    }

    void Update() {
        switch (currentGameplaystate) {
            case GameplayState.BlockSelection:
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
                    selectedBlock = (selectedBlock + 1) % MAX_PROPOSED_BLOCKS;
                }
                else if (Input.GetKeyDown(KeyCode.Space)) {
                    if (gameplayCamera.currentState != GameplayCamera.CameraState.MoveTo) {
                        tower.CreateNextBlockAtID(currentSpotID);
                        currentSpotID = tower.NextNeighbourID(currentSpotID);
                        gameplayCamera.target.y = tower.IDToGameplayPosition(currentSpotID).y + gameplayCamera.initialHeigh;
                        gameplayCamera.MoveToNextTarget();
                        currentGameplaystate = GameplayState.TurnTransition;
                    }
                }
                break;
            case GameplayState.TurnTransition: // G�n�ration d'une nouvelle main de blocks
                currentGameplaystate = GameplayState.BlockSelection;
                ResetHand();
                break;
            default:
                break;
        }
	}

    private void ResetHand() {
        proposedBlocks.Clear();
        for (int i = 0; i < MAX_PROPOSED_BLOCKS; i++) {
            int random = Random.Range(1, System.Convert.ToInt32((Block.BlockType.NumerOfBlockTypes)));
            proposedBlocks.Add((Block.BlockType)random);
        }
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 20), proposedBlocks[selectedBlock].ToString());
    }
    
}
