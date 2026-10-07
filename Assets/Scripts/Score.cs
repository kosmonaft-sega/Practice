using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    //links to components
    public Rigidbody rd;
    
    private List<Vector3> dirs;

    private void Awake() {
        dirs = (List<Vector3>)transform.GetComponent<Variables>().declarations.GetDeclaration("dirs").value;
    }

    public int Score_Count(Vector3 upside)
    {
        var score = 0;
        var best = 0f;
        for(int i=0;i<dirs.Count;i++)
        {
            if(best < Vector3.Dot(upside,dirs[i]))
            {
                best = Vector3.Dot(upside,dirs[i]);
                score = i+1;
            }
        }
        return score;
    }
}
