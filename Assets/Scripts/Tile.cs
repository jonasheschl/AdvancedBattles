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
     * Called when the selected unit is changed.
     */
    private void SelectedUnitChanged(object sender, (Unit oldValue, Unit newValue) e)
    {
        if (e.oldValue == null && e.newValue != null) // Nothing was selected previously, selected a unit now
        {
            if (Utils.WithinRange(this, e.newValue) && LocalUnit == null)
                Highlighter.Highlighted = true;
        }
        else if (e.oldValue != null && e.newValue == null) // A unit was previously selected, clicked an empty tile now
        {
            if (Utils.WithinRange(this, e.oldValue))
                e.oldValue.Move(X, Y);
            else
                // Clicked an unreachable tile, deselect unit
                GlobalData.SelectedUnit = null;
        }
    }

    public void OnMouseDown()
    {
        GlobalData.SelectedUnit = null;
    }
}

public enum TileType
{
    Plain,
    Forrest,
    Mountain
}
