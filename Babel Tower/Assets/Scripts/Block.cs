using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {

    public enum BlockType: int {
        NullBlock = -1,
        Cistern,
        Generator,
        Slum,
        School,
        GreasySpoon,
        Workshop,
        NumerOfBlockTypes
    };

    public enum ResourceType : int {
        NullResource = -1,
        Water,
        Electricity,
        People,
        NumerOfResourceTypes
    };

    public BlockType blockType = BlockType.NullBlock;
    public List<ResourceType> producedResources;
    public List<ResourceType> usedResources;
    public int producedScience = 0;
    public string descriptionText = "";
    public bool active = false;

    
    void Start () {
        producedResources = new List<ResourceType>();
        usedResources = new List<ResourceType>();
    }
	
	// Update is called once per frame
	void Update () {
        
	
	}

    public void CheckActivity(List<Block> _neighborood) {
        bool oneMissing = false;
        foreach (ResourceType used in usedResources) {
            bool isSatisfied = false;
            foreach (Block neighbor in _neighborood) {
                foreach (ResourceType produced in neighbor.producedResources) {
                    if (produced == used) {
                        isSatisfied = true;
                        break;
                    }
                }
                if (isSatisfied)
                    break;
            }
            if (oneMissing)
                break;
        }
        active = !oneMissing;
    }

    public void ChangeType(BlockType _type) {
        blockType = _type;
        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        switch (blockType) {
            case BlockType.Generator:
                rend.material.SetColor("_Color", Color.red);
                producedResources.Add(ResourceType.Electricity);
                descriptionText = "Generator";
                break;
            case BlockType.Cistern:
                rend.material.SetColor("_Color", Color.blue);
                producedResources.Add(ResourceType.Water);
                descriptionText = "Cistern";
                break;
            case BlockType.Slum:
                rend.material.SetColor("_Color", Color.gray);
                producedResources.Add(ResourceType.People);
                descriptionText = "Slum";
                break;
            case BlockType.School:
                rend.material.SetColor("_Color", Color.green);
                producedScience = 1;
                descriptionText = "School";
                break;
            case BlockType.GreasySpoon:
                rend.material.SetColor("_Color", Color.magenta);
                usedResources.Add(ResourceType.Water);
                descriptionText = "GreasySpoon";
                break;
            case BlockType.Workshop:
                rend.material.SetColor("_Color", Color.yellow);
                usedResources.Add(ResourceType.Electricity);
                descriptionText = "Workshop";
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
