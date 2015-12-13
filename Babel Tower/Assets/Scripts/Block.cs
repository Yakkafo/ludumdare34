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
    public int producedMoney = 0;
    public string descriptionText = "";
    public bool active = false;
    public Mesh[] blockMeshes;

    void Awake () {
        producedResources = new List<ResourceType>();
        usedResources = new List<ResourceType>();
    }
	
	// Update is called once per frame
	void Update () {
        
	
	}

    public void CheckActivity(List<Block> _neighborhood) {
        bool oneMissing = false;
        foreach (ResourceType used in usedResources) {
            bool isSatisfied = false;
            foreach (Block neighbor in _neighborhood) {
                foreach (ResourceType produced in neighbor.producedResources) {
                    if (produced == used) {
                        isSatisfied = true;
                        break;
                    }
                }
                if (isSatisfied) 
                    break;
            }
            if (!isSatisfied) {
                oneMissing = true;
                break;
            }
        }
        active = !oneMissing;
    }

    public void ChangeType(BlockType _type) {
        blockType = _type;
        Renderer rend = GetComponent<Renderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = blockMeshes[GetBlockTypeID(blockType)];
        rend.material.shader = Shader.Find("Specular");
        switch (blockType) {
            case BlockType.Generator:
                rend.material.SetColor("_Color", ConvertHexaToUnityRGB(251, 249, 184));
                producedResources.Add(ResourceType.Electricity);
                descriptionText = "Generator";
                break;
            case BlockType.Cistern:
                rend.material.SetColor("_Color", ConvertHexaToUnityRGB(215, 180, 240));
                producedResources.Add(ResourceType.Water);
                descriptionText = "Cistern";
                break;
            case BlockType.Slum:
                rend.material.SetColor("_Color", ConvertHexaToUnityRGB(175, 239, 230));
                producedResources.Add(ResourceType.People);
                descriptionText = "Slum";
                break;
            case BlockType.School:
                rend.material.SetColor("_Color", ConvertHexaToUnityRGB(237, 175, 175));
                usedResources.Add(ResourceType.People);
                producedScience = 1;
                descriptionText = "School";
                break;
            case BlockType.GreasySpoon:
                rend.material.SetColor("_Color", ConvertHexaToUnityRGB(173, 201, 234));
                usedResources.Add(ResourceType.Water);
                descriptionText = "GreasySpoon";
                producedMoney = 1;
                break;
            case BlockType.Workshop:
                rend.material.SetColor("_Color", ConvertHexaToUnityRGB(60, 85, 114));
                usedResources.Add(ResourceType.Electricity);
                descriptionText = "Workshop";
                producedMoney = 1;
                break;
            default:
                rend.material.SetColor("_Color", Color.black);
                break;
        }
    }

    public static BlockType GetBlockType(int _i) {
        return (BlockType)_i;
    }

    public static int GetBlockTypeID(BlockType _type) {
        return System.Convert.ToInt32(_type);
    }

    public static Color ConvertHexaToUnityRGB(float r, float g, float b) {
        return new Color(r / 255f, g / 255f, b / 255f);
    }
}
