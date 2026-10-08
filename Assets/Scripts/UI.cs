using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    //links to objects
    private Database database;
    [SerializeField] private TMP_Text scoreboard;
    [SerializeField] private TMP_Text buttonText;

    [SerializeField] private TMP_InputField cubesField;

    [SerializeField] private TMP_InputField winScoreField;
    [SerializeField] private TMP_InputField drawScoreField;
    [SerializeField] private TMP_InputField loseScoreField;

    private bool isWinScoreSet = false;
    private bool isDrawScoreSet = false;

    public void Init(Database _database)
    {
        database = _database;

        database.diceThrowEvent += WinScoreChange;
        database.diceThrowEvent += DrawScoreChange;
        database.diceThrowEvent += LoseScoreChange;
    }

    private void OnEnable()
    {
        if(database != null)
        {
            database.diceThrowEvent += WinScoreChange;
            database.diceThrowEvent += DrawScoreChange;
            database.diceThrowEvent += LoseScoreChange;
        }
    }

    private void OnDisable()
    {
        if(database != null)
        {
            database.diceThrowEvent -= WinScoreChange;
            database.diceThrowEvent -= DrawScoreChange;
            database.diceThrowEvent -= LoseScoreChange;
        }
    }

    private void Awake() {
        if(scoreboard == null)
            Debug.LogError("Ошибка ссылка на scoreboard не найдена");

        if(buttonText == null)
            Debug.LogError("Ошибка ссылка на текст кнопки не найдена");

        if(cubesField != null)
        {
            cubesField.onEndEdit.AddListener(CubesChange);
            cubesField.text = $"{database.cubes}";
        }
        else
            Debug.LogError("Ошибка ссылка на поле cubesField не найдена");

        if(winScoreField != null)
            winScoreField.onEndEdit.AddListener(SetWinScore);
        else
            Debug.LogError("Ошибка ссылка на поле winScoreField не найдена");

        if(drawScoreField != null)
            drawScoreField.onEndEdit.AddListener(SetDrawScore);
        else
            Debug.LogError("Ошибка ссылка на поле drawScoreField не найдена");

        Set_Score(database.score);
        LoseScoreChange();
    }

    public void Set_Score(int score)
    {
        scoreboard.SetText($"Score: {score}");

        isWinScoreSet = false;
        isDrawScoreSet = false;

        winScoreField.interactable = true;
        drawScoreField.interactable = true;
    }

    public void Set_Key(string key)
    {
        buttonText.SetText("Press to rebind\nKey: "+key);
    }

    private void CubesChange(string score)
    {
        if(score != "")
            database.cubes = int.Parse(score);
        else
        {
            cubesField.text = "1";
            database.cubes = 1;
        }
    }

    private void LoseScoreChange()
    {
        loseScoreField.text = $"{database.cubes}";
    }

    private void WinScoreChange()
    {
        winScoreField.interactable = false;
        string scoreText = winScoreField.text;
        if(isWinScoreSet)
            database.winScore = int.Parse(scoreText);
        else
        {
            int winScore = Random.Range(database.cubes,database.cubes*6);
            winScoreField.text = $"{winScore}";
            database.winScore = winScore;
        }
    }

    private void DrawScoreChange()
    {
        drawScoreField.interactable = false;
        string scoreText = drawScoreField.text;
        if(isDrawScoreSet)
            database.drawScore = int.Parse(scoreText);
        else
        {
            int drawScore = Random.Range(database.cubes,database.winScore);
            drawScoreField.text = $"{drawScore}";
            database.drawScore = drawScore;
        }
    }

    public void SetWinScore(string text) => isWinScoreSet = true;

    public void SetDrawScore(string text) => isDrawScoreSet = true;
}
