using UnityEngine;
using UnityEngine.InputSystem;

public class Rebinder : MonoBehaviour
{
    //Set up for rebind
    [SerializeField] private InputActionReference Action;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    [SerializeField] private UI UI;

    private void Update()
    {
        //If action got rebind
        if (rebindOperation != null)
        {
            if (rebindOperation.completed)
            {
                Action.action.Enable();
                UI.Set_Key(rebindOperation.action.bindings[0].effectivePath.Replace("<Keyboard>/", ""));
                rebindOperation = null;
            }
        }
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
}
