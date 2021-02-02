using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TurnManager : MonoBehaviour
{
    [SerializeField] private static Unit.UnitTeam movingTeam;
    public static Unit.UnitTeam MovingTeam
    {
        get => movingTeam;
        private set
        {
            MovingTeamChanged?.Invoke(null, (movingTeam, value));
            movingTeam = value;
        }
    }
    
    public static event EventHandler<(Unit.UnitTeam? oldValue, Unit.UnitTeam newValue)> MovingTeamChanged;

    public static void Turn()
    {
        // Swap moving team
        MovingTeam = Utils.OtherTeam(movingTeam);
    }

    public void Start()
    {
        MovingTeam = Unit.UnitTeam.Blue;
    }
}
