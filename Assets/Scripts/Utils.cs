using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils 
{
    public static Vector3Int CoordToVector(TileCoord coord)
    {
        return new Vector3Int(coord.X, -coord.Y, 0);
    }
}
