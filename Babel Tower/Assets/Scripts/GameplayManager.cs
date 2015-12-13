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

    // Gameplay
    public int playerScience = 0;
    public int playerMoney = 0;

    // Choisir un block
    private List<Block.BlockType> proposedBlocks;
    private int selectedBlock = 0;
    private const int MAX_PROPOSED_BLOCKS = 3;

    // GUI
    public GUISkin guiSkin;
    public Texture2D[] texturesUnfocusedButtons;
    public Texture2D[] texturesFocusedButtons;
    public Texture2D textureLabel;
    private Rect topRect;

	void Start () {
        topRect = new Rect(10, 10, 64, 64);
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
                        tower.CreateNextBlockAtID(currentSpotID, proposedBlocks[selectedBlock]);
                        currentSpotID = tower.NextNeighbourID(currentSpotID);
                        if (gameplayCamera.target.y < tower.GetSpot(currentSpotID).gameplayPosition.y)
                            gameplayCamera.target.y = tower.GetSpot(currentSpotID).gameplayPosition.y; // On met à jour la target de la caméra
                        gameplayCamera.MoveToNextTarget();
                        currentGameplaystate = GameplayState.TurnTransition;
                    }
                }
                break;
            case GameplayState.TurnTransition: // Génération d'une nouvelle main de blocks et calcule des gains
                int totalScience = 0;
                foreach(Spot spot in tower.towerSpots) {
                    if(spot.containedBlock != null) {
                        List<Block> neighborhood = tower.GetNeighborhood(spot.id);
                        spot.containedBlock.CheckActivity(neighborhood);
                        if (spot.containedBlock.active) {
                            playerMoney += spot.containedBlock.producedMoney;
                            if(spot.containedBlock.producedMoney > 0) {
                                spot.containedBlock.GetComponent<ParticleSystem>().Play();
                            }
                            totalScience += spot.containedBlock.producedScience;
                        }
                    }
                }
                playerScience = totalScience;

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
            int random = Random.Range(0, Block.GetBlockTypeID(Block.BlockType.NumerOfBlockTypes));
            proposedBlocks.Add((Block.BlockType)random);
        }
    }

    void OnGUI() {
        GUI.skin = guiSkin;
        Rect labelRect = new Rect(Screen.width / 2 - 100, 10, 200, 60);
        GUI.Box(labelRect, "Money: " + playerMoney);
        labelRect.y += 62;
        GUI.Box(labelRect, "Science: " + playerScience);
        Rect currentButtonRect = new Rect(topRect);
        for (int i = MAX_PROPOSED_BLOCKS - 1; i >= 0; i--) {
            currentButtonRect.y += 70;
            if(i == selectedBlock)
                GUI.Box(currentButtonRect, texturesFocusedButtons[Block.GetBlockTypeID(proposedBlocks[i])]);
            else
                GUI.Box(currentButtonRect, texturesUnfocusedButtons[Block.GetBlockTypeID(proposedBlocks[i])]);
            
        }
    }
    
}
