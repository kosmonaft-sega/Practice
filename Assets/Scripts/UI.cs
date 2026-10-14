using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //links to objects
    [SerializeField] private Database database;

    [SerializeField] private Button throwButton;

    [SerializeField] private TMP_Text scoreboard;
    [SerializeField] private TMP_Text winOrLose;

    [SerializeField] private TMP_InputField cubesField;

    [SerializeField] private TMP_InputField winScoreField;
    [SerializeField] private TMP_InputField drawScoreField;
    [SerializeField] private TMP_InputField loseScoreField;

    [SerializeField] private Toggle winToggle;
    [SerializeField] private Toggle drawToggle;

    private void AddToEvent()
    {
        if(database != null)
        {
            database.diceThrowEvent += EraseWinStatus;
            database.diceThrowEvent += WinScoreChange;
            database.diceThrowEvent += DrawScoreChange;
            database.diceThrowEvent += LoseScoreChange;

            database.diceThrowEndEvent += SetScore;
        }
    }

    private void Awake() {
        AddToEvent();
        if(scoreboard == null)
            Debug.LogError("Ошибка ссылка на scoreboard не найдена");

        if(cubesField != null)
        {
            cubesField.onEndEdit.AddListener(CubesChange);
            cubesField.text = $"{database.cubes}";
        }
        else
            Debug.LogError("Ошибка ссылка на поле cubesField не найдена");

        if(winToggle != null)
            winToggle.onValueChanged.AddListener(SetWinScore);
        else
            Debug.LogError("Ошибка ссылка на поле winToggle не найдена");

        if(drawToggle != null)
            drawToggle.onValueChanged.AddListener(SetDrawScore);
        else
            Debug.LogError("Ошибка ссылка на поле drawToggle не найдена");

        if(throwButton != null)
            throwButton.onClick.AddListener(database.DiceThrowStart);
        else
            Debug.LogError("Ошибка throwButton не найдена!");
    }

    private void OnDestroy()
    {
        if(database != null)
        {
            database.diceThrowEvent -= EraseWinStatus;
            database.diceThrowEvent -= WinScoreChange;
            database.diceThrowEvent -= DrawScoreChange;
            database.diceThrowEvent -= LoseScoreChange;

            database.diceThrowEndEvent -= SetScore;
        }
    }

    private void SetScore()
    {
        scoreboard.SetText($"Score: {database.score}");
        
        winOrLose.SetText($"{database.endGameResult}");

        winToggle.interactable = true;
        drawToggle.interactable = true;
        winScoreField.interactable = winToggle.isOn;
        drawScoreField.interactable = drawToggle.isOn;
    }

    private void CubesChange(string value)
    {
        if(value != "")
            database.cubes = int.Parse(value);
        else
        {
            cubesField.text = "1";
            database.cubes = 1;
        }
    }

    private void EraseWinStatus()
    {
        winOrLose.text = "";
    }

    private void LoseScoreChange()
    {
        loseScoreField.text = $"{database.cubes}";
    }

    private void WinScoreChange()
    {
        winToggle.interactable = false;
        winScoreField.interactable = false;
        string scoreText = winScoreField.text;
        if(winToggle.isOn)
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
        drawToggle.interactable = false;
        drawScoreField.interactable = false;
        string scoreText = drawScoreField.text;
        if(drawToggle.isOn)
            database.drawScore = int.Parse(scoreText);
        else
        {
            int drawScore = Random.Range(database.cubes+1,database.winScore);
            drawScoreField.text = $"{drawScore}";
            database.drawScore = drawScore;
        }
    }

    public void SetWinScore(bool value) => winScoreField.interactable = value;
    public void SetDrawScore(bool value) => drawScoreField.interactable = value;
}
