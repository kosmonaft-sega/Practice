using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    public List<int> sides;
    private Vector3 upside;
    private int multiplier2 = 2;
    private int multiplier3 = 3;

    public int Score_Count(Vector3 upside)
    {
        sides = (List<int>)transform.GetComponent<Variables>().declarations.GetDeclaration("diceSides").value;
        var score=sides[(int)
            (Math.Round(upside.x)
            +Math.Round(upside.y*multiplier2)
            +Math.Round(upside.z*multiplier3)
            +multiplier3)];
        return score;
    }
}
