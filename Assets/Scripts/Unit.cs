using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    
    [SerializeField] private float attackDamage;
    public float AttackDamage => attackDamage;
    
    /**
     * The movement range of this unit.
     */
    [FormerlySerializedAs("movement")] [SerializeField] private int movementPerTurn;
    public int MovementPerTurn => movementPerTurn;

    public int RemainingTurnMovement { get; private set; }

    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;

    public event EventHandler<(float oldValue, float newValue)> RemainingHealthChanged;
    
    private float _remainingHealth;
    public float RemainingHealth
    {
        get => _remainingHealth;
        private set
        {
            // Invoke the RemainingHealthChanged event
            RemainingHealthChanged?.Invoke(null, (_remainingHealth, value));
            
            // Assign the new amount of remaining health. If this new amount would be below zero, assign zero instead
            if (value >= 0)
                _remainingHealth = value;
            else
                _remainingHealth = 0;
        }
    }

    public Tile LocalTile => Utils.PlayingFieldRoot.TileDataAt(X, Y);

    public bool hasAttacked = false;
    #endregion properties

    private EventHandler<(UnitTeam? oldValue, UnitTeam newValue)> movingTeamChangedHandler;
    
    private void Awake()
    {
        // Set the initial health
        _remainingHealth = maxHealth;
        
        // Adding the delegate to the event must be called before SerializeField variables are initialized.
        // Otherwise the method would not be called when the starting team is assigned to MovingTeam in TurnManager
        //
        // In order for the handler to be able to remove itself whenever its corresponding unit is destroyed, a
        // reference to the handler must be saved in the movingTeamChangedHandler variable.
        movingTeamChangedHandler =  OnMovingTeamChanged;
        TurnManager.MovingTeamChanged += movingTeamChangedHandler;
    }

    private void OnMovingTeamChanged(object sender, (UnitTeam? oldValue, UnitTeam newValue) e)
    {
        // Whenever a unit is destroyed, its corresponding event handler in TurnManager.MovingTeamChanged is not
        // removed. We must thus check for whether or not this (the unit instance) is null. In case it is, the
        // listener removes itself from TurnManger.MovingTeamChanged an returns.
        if (this == null)
        {
            TurnManager.MovingTeamChanged -= movingTeamChangedHandler;
            return;
        }

        // If the current team is this team, the unit can move
        // Otherwise it can not
        var unitEnabled = e.newValue == this.team;

        // En- or disable the units BoxCollider2D to enable/disable it from moving
        gameObject.GetComponent<BoxCollider2D>().enabled = unitEnabled;
        
        // Reset the units RemainingTurnMovement
        RemainingTurnMovement = MovementPerTurn;
        
        // Reset the units ability to attack
        hasAttacked = false;
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

    public void Damage(float damage)
    {
        // Assign the damage
        RemainingHealth = _remainingHealth - damage;
        
        // If this unit has no health anymore, destroy it
        if (RemainingHealth == 0)
            Destroy(this.gameObject);
    }
    
    public void OnMouseDown()
    {
        var selected = GlobalData.SelectedUnit;
        // If no unit is selected, select this clicked unit
        if (selected == null)
            GlobalData.SelectedUnit = this;
    }

    public enum UnitTeam
    {
        Blue,
        Red
    }
}
