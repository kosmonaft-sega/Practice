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
    public event DiceThrow diceThrowEndEvent;

    public enum EndGame
    {
        None,
        Win,
        Draw,
        Lose
    }

    public EndGame endGameResult; 

    public void DiceThrowStart()
    {
        if(isThrown) return;
        diceThrowEvent();
    }
    public void DiceThrowEnd()
    {
        isThrown = false;
        diceThrowEndEvent();
    }
}
