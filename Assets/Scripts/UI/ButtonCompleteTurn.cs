using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCompleteTurn : MonoBehaviour
{
    public void ClickCompleteTurn()
    {
        TurnManager.Turn();
    }
}
