using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public sealed class GlobalData : MonoBehaviour
{
    #region SelectedUnit
    public static event EventHandler<(Unit oldValue, Unit newValue)> SelectedUnitChanged;
    
    /**
     * The GameObject of the currently selected unit.
     */
    [CanBeNull] private static Unit _selectedUnit;
    [CanBeNull] public static Unit SelectedUnit
    {
        get => _selectedUnit;
        set
        {
            SelectedUnitChanged?.Invoke(null, (_selectedUnit, value));
            _selectedUnit = value;
        }
    }
    #endregion SelectedUnit
    
    private GlobalData() { }

}
