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
        computeTowerPosition();
    }

    public void computeTowerPosition() {
        int modulo = id % 4;
        gameplayPosition.x = modulo % 2;
        gameplayPosition.y = modulo / 2;
        gameplayPosition.z = id / 4;
    }

    public int nextNeighbourgID() {
        return 0;
    }

    public override string ToString()
    {
        return "Spot "+ id + ": " + gameplayPosition;
    }
}
