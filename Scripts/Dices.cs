using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dices : MonoBehaviour
{
    //Set up for rebind
    [SerializeField] private InputActionReference Action;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;
    
    //links to objects
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private GameObject buttonText;

    [SerializeField] private GameObject dicePrefab;
    [SerializeField,Range(1,10)] private int cubes;

    //Force control
    [SerializeField,Header("Min|Max Torque")] private Vector2 torque = new Vector2(-200f,200f);
    [SerializeField,Header("Min|Max Up Force|Side force absolutes")] private Vector3 forces = new Vector3(200f,500f,200f);


    //Score counter related
    private int[] sides = new int[7] {4,2,1,0,6,5,3};
    private int multiplier2 = 2;
    private int multiplier3 = 3;
    private int score;

    //Checking if dice moving
    private bool isThrown = false;
    private int notMovingCubes;
    private Vector3 upside;

    //List of dice objects
    private List<GameObject> childrenlist = new List<GameObject>();

    int prev_cubes;

    private void Awake() {
        prev_cubes = cubes;
        if(dicePrefab != null)
        {
            summon_cube();
        }
        else
        {
            Debug.Log("Ошибка пырефаб не найден!");
        }
    }

    private void Update()
    {
        if (prev_cubes != cubes)
        {
            summon_cube();
        }
        prev_cubes = cubes;
        //
        if (isThrown)
        {
            notMovingCubes = 0;
            score = 0;
            foreach(GameObject dice in childrenlist)
            {
                if (dice.GetComponent<Rigidbody>().linearVelocity.Equals(Vector3.zero) && dice.GetComponent<Rigidbody>().angularVelocity.Equals(Vector3.zero))
                {
                    notMovingCubes++;
                    upside = dice.transform.InverseTransformDirection(Vector3.up);
                    score+=sides[(int)
                        (Math.Round(upside.x)
                        +Math.Round(upside.y*multiplier2)
                        +Math.Round(upside.z*multiplier3)
                        +multiplier3)];
                }
            }
            if (notMovingCubes == childrenlist.Count)
            {
                isThrown = false;
                print(score);
                scoreboard.GetComponent<TMP_Text>().SetText("Score: "+score);
            }
        }

        //If action got rebind
        if (rebindOperation != null)
        {
            if (rebindOperation.completed)
            {
                Action.action.Enable();
                buttonText.GetComponent<TMP_Text>().SetText(rebindOperation.action.bindings[0].effectivePath.Replace("<Keyboard>/", ""));
                rebindOperation = null;
            }
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.started && !isThrown)
        {
            Rigidbody rd;
            foreach(GameObject dice in childrenlist)
            {
                rd = dice.GetComponent<Rigidbody>();
                rd.AddForce(new Vector3(
                    UnityEngine.Random.Range(-forces.z, forces.z),
                    UnityEngine.Random.Range(forces.x, forces.y),
                    UnityEngine.Random.Range(-forces.z, forces.z)),ForceMode.Force);
                rd.AddTorque(new Vector3(
                    UnityEngine.Random.Range(torque.x, torque.y),
                    UnityEngine.Random.Range(torque.x, torque.y),
                    UnityEngine.Random.Range(torque.x, torque.y)),ForceMode.Force);
            }
        }
        if (context.canceled)
            isThrown = true;
    }

    public void Rebind()
    {
        Action.action.Disable();
        rebindOperation = Action.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.2f)
            .Start();
    }
    
    private void summon_cube()
    {
        int newdices = cubes-childrenlist.Count;
        GameObject newobject;
        if(newdices>0)
        {
            for(int i=0; i<newdices; i++)
            {
                newobject = Instantiate(dicePrefab, transform.position, Quaternion.identity, transform);
                childrenlist.Add(newobject);
            }
        }
        else if(newdices<0)
        {
            for(int i=0; i<Math.Abs(newdices); i++)
            {
                newobject = childrenlist[i];
                childrenlist.Remove(newobject);
                Destroy(newobject);
            }
        }
    }

}
