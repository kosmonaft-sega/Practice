using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dices : MonoBehaviour
{
    //links to objects
    [SerializeField] private Database database;
    [SerializeField] private GameObject spawner;

    //setup for dices
    [SerializeField] private Score dicePrefab;

    //Force control
    [SerializeField,Header("Min|Max Torque")] private Vector2 torque = new Vector2(-200f,200f);
    [SerializeField,Header("Min|Max Up Force|Side force absolutes")] private Vector3 forces = new Vector3(200f,500f,200f);

    //List of dice objects
    private List<Score> childrenList = new List<Score>();

    private void Awake()
    {
        if(dicePrefab != null && spawner != null && database != null)
            SummonCube();
        else
            Debug.LogError("Ошибка префаб или спавнер или датабаза не найден!");
    }

    private void OnEnable()
    {
        if(database == null)
        {
            Debug.LogError("Ссылка на Database не найдена!");
            return;
        }
        database.diceThrowEvent += Throw;
        database.diceThrowEndEvent += GameResult;
    }

    private void OnDisable()
    {
        if(database == null)
        {
            Debug.LogError("Ссылка на Database не найдена!");
            return;
        }
        database.diceThrowEvent -= Throw;
        database.diceThrowEndEvent -= GameResult;
    }

    private void Throw()
    {
        StartCoroutine(IsCubesThrown());
    }
    
    private void SummonCube()
    {
        int dicesCount = childrenList.Count;
        int newDices = database.cubes - dicesCount;
        if(newDices>0)
        {
            Vector3 spawnAngle = new Vector3(0,360/newDices,0);
            for(int i=0; i<newDices; i++)
            {
                spawner.transform.localEulerAngles = spawnAngle*i;
                childrenList.Add(Instantiate(dicePrefab, spawner.transform.forward*5, Quaternion.identity, transform));
            }
        }   
        else if(newDices<0)
        {
            Score newobject;
            for(int i=0; i<-newDices; i++)
            {
                newobject = childrenList[dicesCount-i-1];
                childrenList.Remove(newobject);
                Destroy(newobject.gameObject);
            }
        }
    }

    private IEnumerator IsCubesThrown()
    {

        int childrenCount = childrenList.Count;
        if(childrenCount != database.cubes)
        {
            SummonCube();
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

        var isCubesStoped = false;

        while (database.isThrown)
        {
            database.score = 0;
            foreach(Score dice in childrenList)
            {
                if (dice.rd.linearVelocity.sqrMagnitude > 0.01f || dice.rd.angularVelocity.sqrMagnitude > 0.01f)
                {
                    isCubesStoped = false;
                    break;
                }
                isCubesStoped = true;
                database.score += dice.Score_Count(dice.transform.InverseTransformDirection(Vector3.up));
            }
            if (isCubesStoped)
                database.DiceThrowEnd();

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void GameResult()
    {
        print(database.score);

        if(database.score>=database.winScore)
            database.endGameResult = Database.EndGame.Win;
        else if(database.score>=database.drawScore)
            database.endGameResult = Database.EndGame.Draw;
        else
            database.endGameResult = Database.EndGame.Lose;
    }

}
