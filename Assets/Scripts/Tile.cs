﻿using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Highlighter))]
public class Tile : MonoBehaviour
{
    #region Properties
    /**
     * X coordinate of this tile. Bottom left corner has coordinates of (0|0).
     */
    [SerializeField] private int x;
    public int X => x;
    
    /**
     * Y coordinate of this tile. Bottom left corner has coordinates of (0|0).
     */
    [SerializeField] private int y;
    public int Y => y;

    [SerializeField] private int defenseRating = 0;
    public int DefenseRating => defenseRating;

    /**
     * Type of this tile.
     */
    [SerializeField] private TileType tileType;
    public TileType TileType => tileType;

    public Highlighter Highlighter => gameObject.GetComponent<Highlighter>();

    [CanBeNull] public Unit LocalUnit => Utils.PlayingFieldRoot.UnitAt(X, Y);
    #endregion Properties

    private void Start()
    {
        GlobalData.SelectedUnitChanged += SelectedUnitChanged;
    }

    /**
     * Handles the highlighting of the tile
     * Called when the selected unit is changed.
     */
    private void SelectedUnitChanged(object sender, (Unit oldValue, Unit newValue) e)
    {
        if (e.newValue != null)
        {
            // A new unit was selected
             if (LocalUnit?.Team == e.newValue.Team &&
                 Utils.WithinRange(this, e.newValue, e.newValue.RemainingTurnMovement))
                // This tile is within movement range of the selected unit and there is a unit on this tile and this
                // unit is of the same team as the selected unit.
                // The selected unit can as such not move here
                Highlighter.HighlightType = HighlightType.None;
            else if (LocalUnit == null && 
                     Utils.WithinRange(this, e.newValue, e.newValue.RemainingTurnMovement))
                // This tile is within movement range of the selected unit and empty 
                Highlighter.HighlightType = HighlightType.Move;
            else if (LocalUnit?.Team != e.newValue.Team &&
                     LocalUnit?.Team != null &&
                     e.newValue.hasAttacked == false &&
                     Utils.WithinRange(this, e.newValue, e.newValue.RemainingTurnMovement + 1))
                // There is a unit on this tile and this unit is not of the same team as the selected unit, the
                // selected unit has not yet attacked during this turn and this tile
                // is within attacking distance of the selected unit.
                // The selected unit can as such not move here, it can instead attack the unit on this tile
                Highlighter.HighlightType = HighlightType.Attack;
        }
        else
        {
            // A unit was deselected of moved
            Highlighter.HighlightType = HighlightType.None;
        }
    }

    /**
     * Clicked this empty tile, if this tile is within moving range of the currently selected unit, move the unit there.
     * Otherwise deselect the unit.
     */
    public void OnMouseDown()
    {
        var selected = GlobalData.SelectedUnit;
        // If a unit is selected
        if (selected != null)
        {
            // If a unit is within range of this tile, move here
            if (Utils.WithinRange(this, selected, selected.RemainingTurnMovement))
                selected.Move(X, Y);

            // If a unit is selected, the selected unit is of an enemy team, has not yet attacked during the current
            // turn and is adjacent to this unit:
            // the selected unit attacks this unit
            if (Utils.IsAdjacent(X, Y, selected.X, selected.Y) &&
                selected.hasAttacked == false &&
                selected.Team != LocalUnit.Team)
            {
                // Calculate and apply the damage dealt by the selected unit to this unit
                var damageDealt = Utils.CalculateDamage(selected.AttackDamage,
                    selected.MaxHealth,
                    this.DefenseRating,
                    LocalUnit.MaxHealth - LocalUnit.RemainingHealth);
                LocalUnit.Damage(damageDealt);

                selected.hasAttacked = true;
            }

            // Regardless to whether or not the unit moved/attacked, deselect it
            GlobalData.SelectedUnit = null;
        }
    }
}

public enum TileType
{
    Plain,
    Forrest,
    Mountain
}
