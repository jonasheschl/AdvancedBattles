using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class PlayingFieldRoot : MonoBehaviour
{
    private GameObject[,] _tiles;
    
    void Start()
    {
        // -=-=- Initialize tiles array -=-=-
        var tilesList = GetComponentsInChildren<Tile>();
        
        var maxX = tilesList
            .Select(x => x.X)
            .Max();
        var maxY = tilesList
            .Select(x => x.Y)
            .Max();

        _tiles = new GameObject[maxX + 1, maxY + 1];
        
        // Insert tile GameObjects into tiles
        foreach (var tileData in tilesList)
        {
            _tiles[tileData.X, tileData.Y] = tileData.gameObject;
        }
    }

    /**
     * Returns the GameObject of the tile with the given coordinates.
     * In case the given coordinates are outside the bounds of the internal 2d-array, return null.
     */
    [CanBeNull]
    public GameObject TileAt(int x, int y)
    {
        // If indices are outside the array bounds, return null
        if (x < 0 || x > _tiles.GetUpperBound(0) ||
            y < 0 || y > _tiles.GetUpperBound(1))
            return null;
        
        return _tiles[x, y];
    }

    [CanBeNull]
    public Tile TileDataAt(int x, int y) =>
        TileAt(x, y)?.GetComponent<Tile>();

    [CanBeNull]
    public Highlighter HighlighterAt(int x, int y) =>
        TileAt(x, y)?.GetComponent<Highlighter>();

    [CanBeNull]
    public Unit UnitAt(int x, int y) =>
        GetComponentsInChildren<Unit>()
            .SingleOrDefault(u => u.X == x && u.Y == y);
}
