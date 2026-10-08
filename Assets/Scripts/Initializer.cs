using UnityEngine;

public class Initializer : MonoBehaviour
{
    //links to objects
    [SerializeField] private Database database;
    [SerializeField] private UI UI;
    [SerializeField] private Dices Dices;

    private void Awake() {
        if(database == null || UI == null || Dices == null)
            Debug.LogError("Не все поля Initializer заполнены");
        else
        {
            UI.Init(database);
            Dices.Init(database, UI);
        }
    }
}
