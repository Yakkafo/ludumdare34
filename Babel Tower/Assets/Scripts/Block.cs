using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public enum BlockType: int {
        Null = 0,
        Water,
        Food,
        NumerOfBlockTypes
    };

    public BlockType blockType = BlockType.Null;

    // Use this for initialization
    void Start () {

        // TODO: temp
        switch(Random.Range(1, 3)) {
            case 1:
                blockType = BlockType.Water;
                break;
            case 2:
                blockType = BlockType.Food;
                break;
        }

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        switch (blockType) {
            case BlockType.Food:
                rend.material.SetColor("_Color", Color.red);
                break;
            case BlockType.Water:
                rend.material.SetColor("_Color", Color.blue);
                break;
            default:
                rend.material.SetColor("_Color", Color.black);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	
	}
}
