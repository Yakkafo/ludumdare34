using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public enum BlockType: int {
        NullBlock = -1,
        Water,
        Food,
        Wood,
        NumerOfBlockTypes
    };

    public BlockType blockType = BlockType.NullBlock;

    // Use this for initialization
    void Start () {

        // TODO: temp
        switch(Random.Range(System.Convert.ToInt32(BlockType.NullBlock) +1, System.Convert.ToInt32(BlockType.NumerOfBlockTypes))) {
            case 0:
                blockType = BlockType.Water;
                break;
            case 1:
                blockType = BlockType.Food;
                break;
            case 2:
                blockType = BlockType.Wood;
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
            case BlockType.Wood:
                rend.material.SetColor("_Color", Color.gray);
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
