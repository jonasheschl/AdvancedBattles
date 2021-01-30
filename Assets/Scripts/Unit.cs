using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
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
     * The movement range of this unit.
     */
    [SerializeField] private int movement;
    public int Movement => movement;
    #endregion properties

    public void Move(int toX, int toY)
    {
        if (Utils.PlayingFieldRoot.UnitAt(toX, toY) != null) return;
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
}
