using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GlobalData
{
    #region SelectedUnit
    public event EventHandler<(GameObject oldValue, GameObject newValue)> SelectedUnitChanged;
    
    /**
     * The GameObject of the currently selected unit.
     */
    [CanBeNull] private GameObject selectedUnit;
    [CanBeNull] public GameObject SelectedUnit
    {
        get => selectedUnit;
        set
        {
            SelectedUnitChanged?.Invoke(null, (selectedUnit, value));
            selectedUnit = value;
        }
    }
    #endregion SelectedUnit

    #region SingletonBoilerplate
    private static GlobalData _instance;

    public static GlobalData Instance =>
        _instance ?? (_instance = new GlobalData());
    #endregion SingletonBoilerplate
}
