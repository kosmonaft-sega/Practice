using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dices : MonoBehaviour
{
    
    //links to objects
    [SerializeField] private TMP_Text scoreboard;

    [SerializeField] private Score dicePrefab;
    [SerializeField,Range(1,10)] private int cubes;

    //Force control
    [SerializeField,Header("Min|Max Torque")] private Vector2 torque = new Vector2(-200f,200f);
    [SerializeField,Header("Min|Max Up Force|Side force absolutes")] private Vector3 forces = new Vector3(200f,500f,200f);


    //Score counter related
    private List<int> sides;
    private int score;

    //Checking if dice moving
    private bool isThrown = false;
    private int notMovingCubes;

    //List of dice objects
    private List<Rigidbody> childrenList = new List<Rigidbody>();

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
            foreach(Rigidbody dice in childrenList)
            {
                if (dice.linearVelocity.Equals(Vector3.zero) && dice.angularVelocity.Equals(Vector3.zero))
                {
                    notMovingCubes++;
                    score += dicePrefab.Score_Count(dice.transform.InverseTransformDirection(Vector3.up));
                }
            }
            if (notMovingCubes == childrenList.Count)
            {
                isThrown = false;
                print(score);
                scoreboard.SetText("Score: "+score);
            }
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.started && !isThrown)
        {
            foreach(Rigidbody dice in childrenList)
            {
                dice.AddForce(new Vector3(
                    UnityEngine.Random.Range(-forces.z, forces.z),
                    UnityEngine.Random.Range(forces.x, forces.y),
                    UnityEngine.Random.Range(-forces.z, forces.z)),ForceMode.Force);
                dice.AddTorque(new Vector3(
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
        int newdices = cubes-childrenList.Count;
        if(newdices>0)
        {
            GameObject newobject;
            for(int i=0; i<newdices; i++)
            {
                newobject = Instantiate(dicePrefab.gameObject, transform.position, Quaternion.identity, transform);
                childrenList.Add(newobject.GetComponent<Rigidbody>());
            }
        }
        else if(newdices<0)
        {
            Rigidbody rd;
            for(int i=0; i<Math.Abs(newdices); i++)
            {
                rd = childrenList[i];
                childrenList.Remove(rd);
                Destroy(rd.gameObject);
            }
        }
    }

}
