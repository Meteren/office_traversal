using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor 
{

    float xPos;
    float yPos;

    public Anchor(float xPos, float yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
    }

    public float XPos { get { return xPos;} }
    public float YPos { get { return yPos; } } 
}
