using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Dices : MonoBehaviour
{
    //links to objects
    private Database database;
    private UI UI;

    //setup for dices
    [SerializeField] private GameObject dicePrefab;

    //Force control
    [SerializeField,Header("Min|Max Torque")] private Vector2 torque = new Vector2(-200f,200f);
    [SerializeField,Header("Min|Max Up Force|Side force absolutes")] private Vector3 forces = new Vector3(200f,500f,200f);

    //List of dice objects
    private List<Score> childrenList = new List<Score>();

    public void Init(Database _database, UI _UI)
    {
        database = _database;
        UI = _UI;
        
        database.diceThrowEvent += Throw;

        if(dicePrefab != null)
            Summon_Cube();
        else
            Debug.LogError("Ошибка префаб не найден!");
    }

    private void OnEnable()
    {
        if(database != null)
            database.diceThrowEvent += Throw;
    }
    private void OnDisable()
    {
        if(database != null)
            database.diceThrowEvent -= Throw;
    }

    public void DiceThrowStart()
    {
        if(database.isThrown)
            return;
        database.DiceThrowStart();
    }

    private void Throw()
    {
        StartCoroutine(IsCubesThrown());
    }
    
    public void Summon_Cube()
    {
        int dicesCount = childrenList.Count;
        int newDices = database.cubes - dicesCount;
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

    public IEnumerator IsCubesThrown()
    {

        int childrenCount = childrenList.Count;
        if(childrenCount != database.cubes)
        {
            Summon_Cube();

            if(childrenCount < database.cubes)
                yield return new WaitForSeconds(1.75f);
        }

        yield return new WaitForSeconds(0.25f);

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
        database.isThrown = true;

        yield return new WaitForSeconds(0.5f);

        int notMovingCubes;

        while (database.isThrown)
        {
            notMovingCubes = 0;
            database.score = 0;
            foreach(Score dice in childrenList)
            {
                if (dice.rd.linearVelocity.sqrMagnitude > 0.01f && dice.rd.angularVelocity.sqrMagnitude > 0.01f)
                    break;
                notMovingCubes++;
                database.score += dice.Score_Count(dice.transform.InverseTransformDirection(Vector3.up));
            }
            if (notMovingCubes == childrenList.Count)
            {
                database.isThrown = false;
                print(database.score);
                UI.Set_Score(database.score);
                if(database.score>=database.winScore)
                    print("Win");
                else if(database.score>=database.drawScore)
                    print("Draw");
                else
                    print("Lose");
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

}
