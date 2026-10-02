using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    public List<int> sides;
    private Vector3 order = new Vector3(1,2,3);

    public int Score_Count(Vector3 upside)
    {
        sides = (List<int>)transform.GetComponent<Variables>().declarations.GetDeclaration("diceSides").value;
        var score=sides[(int)
            (Math.Round((Vector3.Dot(upside,order)))+order.z)];
        return score;
    }
}
