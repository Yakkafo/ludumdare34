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
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
