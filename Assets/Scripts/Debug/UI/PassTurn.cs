using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTurn : MonoBehaviour
{
    public void PassTurnButton()
    {
        TurnManager.Instance.actionCounter = 3;
    }
}
