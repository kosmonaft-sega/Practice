using UnityEngine;

public class Database : MonoBehaviour
{
    public int cubes;
    public int score;
    public int loseScore;
    public int drawScore;
    public int winScore;

    //Checking if dice moving
    public bool isThrown = false;
  
    public delegate void DiceThrow();
    public event DiceThrow diceThrowEvent;

    public void DiceThrowStart()
    {
        diceThrowEvent();
    }
}
