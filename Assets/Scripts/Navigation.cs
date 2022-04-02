using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Navigation : MonoBehaviour
{



    bool Navigate(TileCoord start, TileCoord end)
    {
        var openSet = new HashSet<TileCoord>();

        var cameFrom = new Dictionary<TileCoord, TileCoord>();

        return false;
    }

    private float heuristicLine(TileCoord a, TileCoord b)
    {
        var dX = a.X - b.X;
        var dY = a.Y - b.Y;
        return Mathf.Sqrt(dX * dX + dY * dY);
    }
}
