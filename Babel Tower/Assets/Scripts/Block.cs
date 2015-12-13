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
    private Renderer rend;
    private Color inactiveColor;
    private Color activeColor;
    private Color currentColor;
    private Vector3 positionShift;
    public Vector3 gameplayPosition;
    private float initialTime = 0f;

    void Awake () {
        producedResources = new List<ResourceType>();
        usedResources = new List<ResourceType>();
        rend = GetComponent<Renderer>();
        positionShift = Vector3.zero;
        initialTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (active) {
            float period = Mathf.Cos(Time.time + initialTime);
            currentColor = Color.Lerp(inactiveColor, activeColor, Mathf.Abs(period));
            positionShift.y = Mathf.Cos((initialTime + Time.time) * 10f) * 0.01f;
            transform.position = gameplayPosition + positionShift;
        }
        else {
            currentColor = inactiveColor;
            transform.position = gameplayPosition;
        }
        rend.material.SetColor("_Color", currentColor);
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
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = blockMeshes[GetBlockTypeID(blockType)];
        switch (blockType) {
            case BlockType.Generator:
                inactiveColor = ConvertHexaToUnityRGB(251, 249, 184);
                producedResources.Add(ResourceType.Electricity);
                descriptionText = "Generator";
                break;
            case BlockType.Cistern:
                inactiveColor = ConvertHexaToUnityRGB(215, 180, 240);
                producedResources.Add(ResourceType.Water);
                descriptionText = "Cistern";
                break;
            case BlockType.Slum:
                inactiveColor = ConvertHexaToUnityRGB(175, 239, 230);
                producedResources.Add(ResourceType.People);
                descriptionText = "Slum";
                break;
            case BlockType.School:
                inactiveColor = ConvertHexaToUnityRGB(237, 175, 175);
                usedResources.Add(ResourceType.People);
                producedScience = 1;
                descriptionText = "School";
                break;
            case BlockType.GreasySpoon:
                inactiveColor = ConvertHexaToUnityRGB(173, 201, 234);
                usedResources.Add(ResourceType.Water);
                descriptionText = "Greasy Spoon";
                producedMoney = 1;
                break;
            case BlockType.Workshop:
                inactiveColor = ConvertHexaToUnityRGB(60, 85, 114);
                usedResources.Add(ResourceType.Electricity);
                descriptionText = "Workshop";
                producedMoney = 1;
                break;
            default:
                inactiveColor = Color.black;
                break;
        }
        Vector3 HSVBuffer = Vector3.zero;
        Color.RGBToHSV(inactiveColor, out HSVBuffer.x, out HSVBuffer.y, out HSVBuffer.z);
        HSVBuffer.y = Mathf.Clamp(3f * HSVBuffer.y, 0f, 1f);
        activeColor = Color.HSVToRGB(HSVBuffer.x, HSVBuffer.y, HSVBuffer.z);

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
