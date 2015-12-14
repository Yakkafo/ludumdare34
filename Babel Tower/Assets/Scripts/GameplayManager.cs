using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour {

    public enum GameplayState {
        Zero = 0,
        BlockSelection,
        TurnTransition,
        EndGame,
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
    public GUISkin buttonSkin;
    public GUISkin veryLongTextSkin;
    public GUISkin signatureSkin;
    public Texture2D[] texturesUnfocusedButtons;
    public Texture2D[] texturesFocusedButtons;

	void Start () {
        proposedBlocks = new List<Block.BlockType>();
        ResetHand();
    }

    void Update() {
        switch (currentGameplaystate) {
            case GameplayState.BlockSelection:
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    selectedBlock = (selectedBlock + 1) % MAX_PROPOSED_BLOCKS;
                }
                else if (Input.GetKeyDown(KeyCode.Space)) {
                    if (gameplayCamera.currentState != GameplayCamera.CameraState.MoveTo) {
                        tower.CreateNextBlockAtID(currentSpotID, proposedBlocks[selectedBlock]);
                        currentSpotID = tower.NextNeighbourID(currentSpotID);
                        //if (gameplayCamera.target.y < tower.GetSpot(currentSpotID).gameplayPosition.y)
                          //  gameplayCamera.target.y = tower.GetSpot(currentSpotID).gameplayPosition.y; // On met à jour la target de la caméra
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
                if (tower.IsFull())
                    currentGameplaystate = GameplayState.EndGame;
                break;
            case GameplayState.EndGame:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                break;
            default:
                break;
        }
	}

    private void ResetHand() {
        proposedBlocks.Clear();
        selectedBlock = 0;
        for (int i = 0; i < MAX_PROPOSED_BLOCKS; i++) {
            int random;
            Block.BlockType block;
            do {
                random = Random.Range(0, Block.GetBlockTypeID(Block.BlockType.NumerOfBlockTypes));
                block = Block.GetBlockType(random);
            } while (proposedBlocks.Contains(block) || playerScience < Block.GetNeededLevel(block)) ;
            proposedBlocks.Add(block);
        }
    }

    void OnGUI() {
        int margin = (Screen.width / 2) - 400;
        int buttonSize = 100;
        Rect boxRect = new Rect(margin + buttonSize + 2, 10, 250, 60);
        GUI.skin = boxSkin;
        GUI.Box(boxRect, "Money: " + playerMoney + "HK$");
        boxRect.x += boxRect.width + 2;
        GUI.Box(boxRect, "Income: +" + playerRevenue + "HK$");
        boxRect.x += boxRect.width + 2;
        boxRect.width = 150;
        GUI.Box(boxRect, "Level: " + playerScience);
        if (currentGameplaystate != GameplayState.EndGame) {
            boxRect.y = boxRect.height + 12;
            boxRect.width = boxRect.height = buttonSize;
            for (int i = MAX_PROPOSED_BLOCKS - 1; i >= 0; i--) {
                if (Block.GetNeededLevel(proposedBlocks[i]) > 0) {
                    boxRect.x = margin - buttonSize / 3 - 2;
                    boxRect.width = buttonSize / 3;
                    GUI.skin = veryLongTextSkin;
                    GUI.Box(boxRect, "L\nV\nL\n" + Block.GetNeededLevel(proposedBlocks[i]));
                }
                boxRect.x = margin;
                boxRect.width = boxRect.height = buttonSize;
                if (i == selectedBlock) {
                    GUI.skin = buttonSkin;
                    GUI.Box(boxRect, texturesFocusedButtons[Block.GetBlockTypeID(proposedBlocks[i])]);
                    boxRect.x += boxRect.width + 2;
                    boxRect.width = 300;
                    GUI.skin = longTextSkin;
                    GUI.Box(boxRect, Block.GetDescription(proposedBlocks[i]));
                    boxRect.width = buttonSize;
                    boxRect.x = margin;
                }
                else {
                    GUI.skin = buttonSkin;
                    GUI.Box(boxRect, texturesUnfocusedButtons[Block.GetBlockTypeID(proposedBlocks[i])]);
                }
                boxRect.y += boxRect.height + 2;
            }
            GUI.skin = veryLongTextSkin;
            boxRect.height /= 2;
            GUI.Box(boxRect, "UP to select another block");
            boxRect.y += boxRect.height + 2;
            GUI.Box(boxRect, "SPACEBAR to build a block");
        } else {
            GUI.skin = boxSkin;
            boxRect.x = margin + buttonSize + 2;
            boxRect.y = 72;
            boxRect.width = 654;
            boxRect.height = 60;
            GUI.Box(boxRect, "The Kowloon Walled City is finished.");
            boxRect.y += boxRect.height + 2;
            GUI.Box(boxRect, "Your score: "+ (int) ((playerScience * playerRevenue) + playerMoney));
            GUI.skin = longTextSkin;
            boxRect.y = Screen.height - boxRect.height - 2;
            boxRect.x += 200;
            boxRect.width -= 400;
            boxRect.height /= 2;
            GUI.Box(boxRect, "SPACEBAR to demolish");
        }
        
    }
    
}
