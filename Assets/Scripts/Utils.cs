using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public static class Utils
{
    /**
     * Returns whether or not the given tile is within the range of the given unit.
     */
    public static bool WithinRange(Tile tile, Unit unit, int range)
    {
        return Distance(tile.X, tile.Y, unit.X, unit.Y) <= range - 1;
    }

    /**
     * Returns the amount of cells between the two given cells in a grid.
     */
    public static int Distance(int x1, int y1, int x2, int y2)
    {
        return Math.Max(Math.Abs(y2 - y1), Math.Abs(x2 - x1));
    }
    
    /**
     * The root of the playing field.
     */
    private static PlayingFieldRoot _playingFieldRoot;
    public static PlayingFieldRoot PlayingFieldRoot {
        get
        {
            if (_playingFieldRoot == null)
            {
                _playingFieldRoot =
                    GameObject.FindWithTag(Constants.TagPlayingField).GetComponentInChildren<PlayingFieldRoot>();
            }
            return _playingFieldRoot;
        }
    }
    
    /**
     * The Controller GameObject.
     */
    private static GameObject _controller;
    public static GameObject Controller {
        get
        {
            if (_controller == null)
            {
                _controller =
                    GameObject.FindWithTag(Constants.TagUI_CompleteTurnButton);
            }
            return _controller;
        }
    }

    public static List<Unit> Units() =>
        PlayingFieldRoot
            .GetComponentsInChildren<Unit>()
            .ToList();

    public static List<Unit> UnitsOfTeam(Unit.UnitTeam team) =>
        Units()
            .Where(unit => unit.Team == team)
            .ToList();

    public static Unit.UnitTeam OtherTeam(Unit.UnitTeam team)
    {
        if (team == Unit.UnitTeam.Blue)
        {
            return Unit.UnitTeam.Red;
        }
        else
        {
            return Unit.UnitTeam.Blue;
        }
    }

    /**
     * Return true if the two given points are adjacent to each other. Returns false otherwise.
     */
    public static bool IsAdjacent(int x1, int y1, int x2, int y2) =>
        Math.Abs(x1 - x2) <= 1 && Math.Abs(y1 - y2) <= 1;

    /**
     * Calculates the amount of damage dealt.
     * See also: https://advancewars.fandom.com/wiki/Damage_Formula
     *
     * attackerDamage: The percentual damage dealt by the attacking unit to the defending unit
     * attackerRemainingHealth: The current amount of health of the attacking unit
     * defenseRating: The defense rating (amount of stars) of the tile the defender is located on
     * defenderLostHealth: The amount of health the defender has already lost
     */
    public static float CalculateDamage(float attackerDamage,
        float attackerRemainingHealth,
        int defenseRating,
        float defenderLostHealth)
    {
        var totalDamage = attackerDamage * (attackerRemainingHealth * 0.1f);
        return totalDamage - defenseRating * ((totalDamage * 0.01f) - (totalDamage * 0.01f * defenderLostHealth));
    }
}
