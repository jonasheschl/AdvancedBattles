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

    public static List<Unit> Units()
    {
        return PlayingFieldRoot
            .GetComponentsInChildren<Unit>()
            .ToList();
    }

    public static List<Unit> UnitsOfTeam(Unit.UnitTeam team)
    {
        return Units()
            .Where(unit => unit.Team == team)
            .ToList();
    }

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

}
