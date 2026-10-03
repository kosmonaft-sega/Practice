using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dices : MonoBehaviour
{
    
    //links to objects
    [SerializeField] private UI UI;

    //setup for dices
    [SerializeField] private GameObject dicePrefab;
    [SerializeField,Range(1,10)] private int cubes;

    //Force control
    [SerializeField,Header("Min|Max Torque")] private Vector2 torque = new Vector2(-200f,200f);
    [SerializeField,Header("Min|Max Up Force|Side force absolutes")] private Vector3 forces = new Vector3(200f,500f,200f);


    //Score counter related
    private int score;

    //Checking if dice moving
    private bool isThrown = false;
    private int notMovingCubes;

    //List of dice objects
    private List<Score> childrenList = new List<Score>();

    int prev_cubes;

   public int Cubes
    {
        get => cubes;
        set
        {
            cubes = value;
            prev_cubes = cubes;
            if(dicePrefab != null)
                Summon_Cube();
            else
                Debug.Log("Ошибка префаб не найден!");
        }
    }

    private void Awake() {
        Cubes = cubes;
    }

    private void Update()
    {
        if (prev_cubes != cubes)
        {
            Cubes = cubes;
        }
        //
        if (isThrown)
        {
            notMovingCubes = 0;
            score = 0;
            foreach(Score dice in childrenList)
            {
                if (dice.rd.linearVelocity.sqrMagnitude < 0.01f && dice.rd.angularVelocity.sqrMagnitude < 0.01f)
                {
                    notMovingCubes++;
                    score += dice.Score_Count(dice.transform.InverseTransformDirection(Vector3.up));
                }
            }
            if (notMovingCubes == childrenList.Count)
            {
                isThrown = false;
                print(score);
                UI.Set_Score(score);
            }
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.started && !isThrown)
        {
            foreach(Score dice in childrenList)
            {
                dice.rd.AddForce(new Vector3(
                    UnityEngine.Random.Range(-forces.z, forces.z),
                    UnityEngine.Random.Range(forces.x, forces.y),
                    UnityEngine.Random.Range(-forces.z, forces.z)),ForceMode.Force);
                dice.rd.AddTorque(new Vector3(
                    UnityEngine.Random.Range(torque.x, torque.y),
                    UnityEngine.Random.Range(torque.x, torque.y),
                    UnityEngine.Random.Range(torque.x, torque.y)),ForceMode.Force);
            }
        }
        if (context.canceled)
            isThrown = true;
    }
    
    private void Summon_Cube()
    {
        int dicesCount = childrenList.Count;
        int newDices = cubes - dicesCount;
        if(newDices>0)
        {
            GameObject newobject;
            for(int i=0; i<newDices; i++)
            {
                newobject = Instantiate(dicePrefab.gameObject, transform.position, Quaternion.identity, transform);
                childrenList.Add(newobject.GetComponent<Score>());
            }
        }
        else if(newDices<0)
        {
            Score newobject;
            for(int i=0; i<Math.Abs(newDices); i++)
            {
                newobject = childrenList[dicesCount-i-1];
                childrenList.Remove(newobject);
                Destroy(newobject.gameObject);
            }
        }
    }

}
