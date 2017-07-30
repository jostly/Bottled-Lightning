using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ActionType
{
    Nothing, GoToNext, GoToPrevious, PickUp, Drop
}

[System.Serializable]
public enum Action
{
    Up, Down, Left, Right
}

public class Station : MonoBehaviour
{
    public Station next;
    public Station previous;

    public ActionType up;
    public ActionType down;
    public ActionType left;
    public ActionType right;

    public PowerSupply linkedPowerSupply;

    public void TakeAction(Character character, Action action)
    {
        switch (action)
        {
            case Action.Up:
                TakeAction(character, action, up);
                break;
            case Action.Down:
                TakeAction(character, action, down);
                break;
            case Action.Left:
                TakeAction(character, action, left);
                break;
            case Action.Right:
                TakeAction(character, action, right);
                break;
        }

    }

    private void TakeAction(Character character, Action action, ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.GoToNext:
                if (next != null)
                {
                    character.MoveToStation(next);
                }
                break;
            case ActionType.GoToPrevious:
                if (previous != null)
                {
                    character.MoveToStation(previous);
                }
                break;
            case ActionType.PickUp:
                character.WaitForPickup(action);
                break;
            case ActionType.Drop:
                character.DropOff(action);
                break;
        }

    }

}
