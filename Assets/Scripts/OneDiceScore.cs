using System.Collections.Generic;
using UnityEngine;

public class OneDiceScore : MonoBehaviour
{
    //links to components
    public Rigidbody rd;
    
    [SerializeField] private List<Vector3> dirs;

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
