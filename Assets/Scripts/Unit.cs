using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    #region properties
    /**
     * Current x position of this unit on the playing field.
     */
    [SerializeField] private int x;
    public int X => x;

    /**
     * Current y position of this unit on the playing field.
     */
    [SerializeField] private int y;
    public int Y => y;

    /**
     * Team of this unit.
     */
    [SerializeField] private UnitTeam team;
    public UnitTeam Team => team;
    
    /**
     * The movement range of this unit.
     */
    [FormerlySerializedAs("movement")] [SerializeField] private int movementPerTurn;
    public int MovementPerTurn => movementPerTurn;

    public int RemainingTurnMovement { get; private set; }
    #endregion properties

    private void Awake()
    {
        // Adding the delegate to the event must be called before SerializeField variables are initialized.
        // Otherwise the method would not be called when the starting team is assigned to MovingTeam in TurnManager
        TurnManager.MovingTeamChanged += OnMovingTeamChanged;
    }

    private void OnMovingTeamChanged(object sender, (UnitTeam? oldValue, UnitTeam newValue) e)
    {
        // If the current team is this team, the unit can move
        // Otherwise it can not
        var unitEnabled = e.newValue == this.team;

        // En- or disable the units BoxCollider2D to enable/disable it from moving
        gameObject.GetComponent<BoxCollider2D>().enabled = unitEnabled;
        
        // Reset the units RemainingTurnMovement
        RemainingTurnMovement = MovementPerTurn;
    }

    public void Move(int toX, int toY)
    {
        // If there is a unit at the location to move to: abort
        if (Utils.PlayingFieldRoot.UnitAt(toX, toY) != null) return;
        
        // Reduce the RemainingTurnMovement by the amount of tiles moved
        var tilesMoved = Utils.Distance(x, y, toX, toY);
        RemainingTurnMovement -= tilesMoved;
            
        // Set playing field location
        x = toX;
        y = toY;
            
        // Calculate new local position based on new location on the playing field
        var newPosition = gameObject.transform.localPosition;
        newPosition.x = toX * Constants.TileHeight;
        newPosition.y = toY * Constants.TileHeight;

        // Apply newly calculated position
        gameObject.transform.localPosition = newPosition;
        
        // Moving done, deselect unit
        GlobalData.SelectedUnit = null;
    }
    
    public void OnMouseDown()
    {
        if (GlobalData.SelectedUnit == null)
            GlobalData.SelectedUnit = this;
    }

    public enum UnitTeam
    {
        Blue,
        Red
    }
}
