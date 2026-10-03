using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{

    //links to objects
    [SerializeField] private TMP_Text scoreboard;
    [SerializeField] private TMP_Text buttonText;

    public void Set_Score(int score)
    {
        scoreboard.SetText("Score: "+score);
    }

    public void Set_Key(string key)
    {
        buttonText.SetText("Press to rebind\nKey: "+key);
    }
}
