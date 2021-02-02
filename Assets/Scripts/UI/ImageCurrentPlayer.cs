using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageCurrentPlayer : MonoBehaviour
{
    [SerializeField] private Sprite blueTurnSprite;
    [SerializeField] private Sprite redTurnSprite;
    
    void Awake()
    {
        // Register event handler to update current player graphic whenever the current player changes.
        // Adding the delegate to the event must be called before SerializeField variables are initialized.
        // Otherwise the method would not be called when the starting team is assigned to MovingTeam in TurnManager.
        TurnManager.MovingTeamChanged += OnMovingTeamChanged;
    }

    private void OnMovingTeamChanged(object sender, (Unit.UnitTeam? oldValue, Unit.UnitTeam newValue) e)
    {
        var image = gameObject.GetComponent<Image>();

        if (e.newValue == Unit.UnitTeam.Blue)
            image.sprite = blueTurnSprite;
        else if (e.newValue == Unit.UnitTeam.Red)
            image.sprite = redTurnSprite;
    }
}
