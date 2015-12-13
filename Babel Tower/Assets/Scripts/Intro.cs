using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

    public GUISkin guiSkin;
    public GUISkin signatureSkin;
    public GUISkin longTextSkin;

    private int diapoID = 0;

    void Start () {
    }
	
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
            diapoID++;
        if (diapoID > 6)
            SceneManager.LoadScene("Gameplay");
	}

    void OnGUI() {
        GUI.skin = guiSkin;
        Rect boxRect = new Rect(Screen.width / 2 - 250, 10, 500, 200);
        GUI.Box(boxRect, "K O W L O O N\nW A L L E D\nC I T Y");

        GUI.skin = longTextSkin;
        boxRect.y += boxRect.height + 2;
        boxRect.height = 400;
        string text = "";
        switch (diapoID) {
            case 0:
                text = "Press SPACEBAR to start";
                break;
            case 1:
                text = "Hong Kong, 1987.";
                break;
            case 2:
                text = "In the heart of New Kowloon, the Walled City stands.";
                break;
            case 3:
                text = "An ancient Chinese military fort which slowly turned into a settlement of 50,000 residents within 0.026 km².";
                break;
            case 4:
                text = "It's 1,923,076 inhabitants per km².";
                break;
            case 5:
                text = "The highest population density in the world.";
                break;
            case 6:
                text = "Build it.";
                break;
            default:
                break;
        }
        GUI.Label(boxRect, text);

        GUI.skin = signatureSkin;
        boxRect.y = Screen.height - 30;
        boxRect.width = 200;
        boxRect.x = Screen.width - boxRect.width - 2;
        boxRect.height = 28;
        GUI.Label(boxRect, "A LD34 game by Yakkafo");
    }
}
