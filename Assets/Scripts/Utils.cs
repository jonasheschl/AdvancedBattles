using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public static class Utils
{
    /**
     * Returns whether or not the given tile is within the range of the given unit.
     */
    public static bool WithinRange(Tile tile, Unit unit)
    {
        return Distance(tile.X, tile.Y, unit.X, unit.Y) <= unit.Movement - 1;
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

}
