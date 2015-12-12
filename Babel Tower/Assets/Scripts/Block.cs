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

        
    }
	
	// Update is called once per frame
	void Update () {
        
	
	}

    public void ChangeType(BlockType _type) {
        blockType = _type;
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

    public static BlockType GetBlockTypeFromInt(int _i) {
        return (BlockType)_i;
    }

    public static int GetIntfromBlockType(BlockType _type) {
        return System.Convert.ToInt32(_type);
    }
}
