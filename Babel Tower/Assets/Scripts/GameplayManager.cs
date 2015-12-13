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
    public int playerRevenue = 0;

    // Choisir un block
    private List<Block.BlockType> proposedBlocks;
    private int selectedBlock = 0;
    private const int MAX_PROPOSED_BLOCKS = 3;

    // GUI
    public GUISkin boxSkin;
    public GUISkin longTextSkin;
    public Texture2D[] texturesUnfocusedButtons;
    public Texture2D[] texturesFocusedButtons;

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
                playerRevenue = 0;
                foreach (Spot spot in tower.towerSpots) {
                    if(spot.containedBlock != null) {
                        List<Block> neighborhood = tower.GetNeighborhood(spot.id);
                        spot.containedBlock.CheckActivity(neighborhood);
                        if (spot.containedBlock.active) {
                            playerRevenue += spot.containedBlock.producedMoney;
                            if(spot.containedBlock.producedMoney > 0) {
                                //spot.containedBlock.GetComponent<ParticleSystem>().Play();
                            }
                            totalScience += spot.containedBlock.producedScience;
                        }
                    }
                }
                playerScience = totalScience;
                playerMoney += playerRevenue;
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
        int margin = (Screen.width / 2) - 400;
        int buttonSize = 100;
        GUI.skin = boxSkin;

        Rect boxRect = new Rect(margin + buttonSize + 2, 10, 250, 60);
        GUI.Box(boxRect, "Money: " + playerMoney + "HK$");
        boxRect.x += boxRect.width + 2;
        GUI.Box(boxRect, "Income: +" + playerRevenue + "HK$");
        boxRect.x += boxRect.width + 2;
        boxRect.width = 150;
        GUI.Box(boxRect, "Level: " + playerScience);
        boxRect.x = margin;
        boxRect.y = boxRect.height + 12;
        boxRect.width = boxRect.height = buttonSize;
        for (int i = MAX_PROPOSED_BLOCKS - 1; i >= 0; i--) {
            if (i == selectedBlock) {
                GUI.skin = boxSkin;
                GUI.Box(boxRect, texturesFocusedButtons[Block.GetBlockTypeID(proposedBlocks[i])]);
                boxRect.x += boxRect.width + 2;
                boxRect.width = 300;
                GUI.skin = longTextSkin;
                GUI.Box(boxRect, Block.GetDescription(proposedBlocks[i]));
                boxRect.width = buttonSize;
                boxRect.x -= boxRect.width + 2;
            }
            else
                GUI.Box(boxRect, texturesUnfocusedButtons[Block.GetBlockTypeID(proposedBlocks[i])]);
            boxRect.y += boxRect.height + 2;
        }
    }
    
}
