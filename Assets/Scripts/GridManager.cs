using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct TileCoord
{
    public TileCoord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return X + "x" + Y;
    }

    public override int GetHashCode()
    {
        return X * 1000 + Y;
    }

    public override bool Equals(object obj)
    {
        if(obj is TileCoord)
        {
            var other = (TileCoord)obj;
            return other.X == X && other.Y == Y;
        }
        return false;
    }

    public int X;
    public int Y;
}

public struct TileInfo
{
    public bool Blocking;
    public bool Burning;
}


public enum TileLayer
{
    UNKNOWN, GROUND, WALL, OBJECT
}

public enum TileType
{
    EMPTY = -1,
    LAVA = 0,
    GROUND = 1,
    BRIDGE = 2,
    WALL = 3,
    PILLAR = 4,
    PIT = 5,
    SPIKES = 6,
    ENEMY_RANGED = 7,
    ENEMY_MELEE = 8,
    CHEST = 9,
    URN = 10,
    STATUE = 11,
    PRESSURE_PLATE = 12

}


public class GridManager : MonoBehaviour
{
    static public GridManager Instance;

    static public Dictionary<TileType, TileInfo> TileInfo = new Dictionary<TileType, TileInfo>()
    {
        {TileType.EMPTY, new TileInfo{} },
        {TileType.LAVA, new TileInfo{Burning = true } },
        {TileType.GROUND, new TileInfo{} },
        {TileType.BRIDGE, new TileInfo{} },
        {TileType.WALL, new TileInfo{Blocking = true} }
    };


    public TileBase lol;
    private int SizeX = 10;
    private int SizeY = 10;

    Dictionary<TileLayer, Tilemap> Tilemaps;
    Grid Grid;

    [Serializable]
    public struct TileObjectPrefab
    {
        public TileType tileType;
        public GameObject prefab;
    }
    [SerializeField]
    TileObjectPrefab[] TileObjectPrefabs;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate singleton GridManager found");
            Destroy(this);
        }
        Grid = FindObjectOfType<Grid>();
        if(Grid == null)
        {
            Debug.LogError("Grid not found");
            return;
        }
        var tileMaps = FindObjectsOfType<Tilemap>();

        Tilemaps = new Dictionary<TileLayer, Tilemap>();
        foreach (var map in tileMaps)
        {
            var name = map.gameObject.name;
            TileLayer layer = TileLayer.UNKNOWN;
            if (name == "wall")
            {
                layer = TileLayer.WALL;
            }
            else if (name == "bridge" || name == "interactable")
            {
                layer = TileLayer.OBJECT;

            }
            else if (name == "ground")
            {
                layer = TileLayer.GROUND;
            }
            else
            {
                Debug.LogError("Unknown tile layer: " + name);
            }
            Tilemaps[layer] = map;
            //SizeX = Mathf.Max(map.size.x);
            //SizeY = Mathf.Max(map.size.y);
        }
        InstantiateObjects();
    }



    private void InstantiateObjects()
    {
        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                var coord = new TileCoord(x, y);
                var objectTile = GetTile(coord, TileLayer.OBJECT);
                var prefab = GetPrefabForTile(objectTile);
                if (prefab == null)
                {
                    var groundTile = GetTile(coord, TileLayer.GROUND);
                    if (groundTile == TileType.LAVA)
                    {
                        Debug.Log(coord);
                    }
                    prefab = GetPrefabForTile(groundTile);
                }

                if (prefab == null)
                {
                    continue;
                }
                Instantiate(prefab);
                var position = GetWorldPosFromTile(coord);
                prefab.transform.position = position;
                var renderer = prefab.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 6;
            }
        }
    }

    public TileType GetTile(TileCoord pos, TileLayer layer)
    {
        var vector = Utils.CoordToVector(pos);
        var tile = ((SuperTiled2Unity.SuperTile)Tilemaps[layer].GetTile(vector));
        if(tile == null)
        {
            return TileType.EMPTY;
        }
        var id = tile.m_TileId;
        return (TileType)id;
    }

    public Vector3 GetWorldPosFromTile(TileCoord coord)
    {
        return Grid.CellToWorld(Utils.CoordToVector(coord)) + new Vector3(Grid.cellSize.x, -Grid.cellSize.y)/2;
    }

    public TileCoord GetTilePosFromWorld(Vector3 pos)
    {
        var vec = Grid.WorldToCell(pos);
        return new TileCoord(vec.x, -vec.y);
    }

    GameObject GetPrefabForTile(TileType tileType)
    {
        foreach (var t in TileObjectPrefabs)
        {
            if(t.tileType == tileType)
            {
                return t.prefab;
            }
        }
        return null;
    }

    public bool IsBlocking(TileCoord t)
    {
        var wallTile = GetTile(t, TileLayer.WALL);
        if (TileInfo.ContainsKey(wallTile) && TileInfo[wallTile].Blocking)
        {
            return true;
        }
        var objectTile = GetTile(t, TileLayer.OBJECT);
        if (TileInfo.ContainsKey(objectTile) && TileInfo[objectTile].Blocking)
        {
            return true;
        }

        return false;
    }

    public float GetDangerCost(TileCoord t)
    {
        var groundTile = GetTile(t, TileLayer.GROUND);
        if (groundTile == TileType.LAVA)
        {
            return 3;
        }
        //var objectTile = GetTile(t, TileLayer.OBJECT);
        //if (TileInfo[objectTile].Blocking)
        //{
        //    return true;
        //}

        return 1;
    }

    internal List<NavNode> GetNeighbors(TileCoord f)
    {
        var result = new List<NavNode>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }
                int nX = f.X + i;
                int nY = f.Y + j;
                if(nX < -10 || nY < 0 || nX > 20 || nY >= 10)
                {
                    continue;
                }
                var nCoord = new TileCoord(nX, nY);
                if (IsBlocking(nCoord))
                {
                    continue;
                }
                float cost = GetDangerCost(nCoord);
                // Diagonal
                if (i != 0 && j != 0 )
                {
                    cost *= 1.44f;
                    if (IsBlocking(new TileCoord(nX, f.Y)) || IsBlocking(new TileCoord(f.X, nY)))
                    {
                        continue;
                    }
                }
                result.Add(new NavNode(nCoord, cost));
            }
        }
        return result;
    }
}
