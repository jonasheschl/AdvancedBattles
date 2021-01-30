using JetBrains.Annotations;
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
        if (e.newValue != null && Utils.WithinRange(this, e.newValue)) 
            // A new unit was selected. The selected unit is within moving range of this tile.
            Highlighter.Highlighted = true;
        else
            // A unit was deselected of moved
            Highlighter.Highlighted = false;
    }

    /**
     * Clicked this empty tile, if this tile is within moving range of the currently selected unit, move the unit there.
     * Otherwise deselect the unit.
     */
    public void OnMouseDown()
    {
        var unit = GlobalData.SelectedUnit;
        // If a unit is selected
        if (unit != null)
        {
            // If a unit is within range of this tile, move here
            if (Utils.WithinRange(this, unit))
                unit.Move(X, Y);
            // Regardless to whether or not the unit moved, deselect it
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
