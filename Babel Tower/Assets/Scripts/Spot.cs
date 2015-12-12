using UnityEngine;
using System.Collections;
using System;

public class Spot
{
    public Block containedBlock;
    public Vector3 gameplayPosition;
    public int id = 0;
    
    public Spot(int _id) {
        id = _id;
        ComputeTowerPosition();
    }

    public void ComputeTowerPosition() {
        int modulo = id % 4;
        gameplayPosition.x = modulo % 2;
        gameplayPosition.z = modulo / 2;
        gameplayPosition.y = id / 4;
    }
    
    public bool ContainsBlock()
    {
        return containedBlock != null;
    }

    public override string ToString()
    {
        return "Spot "+ id + ": " + gameplayPosition + ". Contains block: " + ContainsBlock();
    }
}
