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
        if (obj is TileCoord)
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
    public bool Hazard;
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
    PRESSURE_PLATE = 12,
    LEVER = 13,

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
        {TileType.WALL, new TileInfo{Blocking = true} },
        {TileType.LEVER, new TileInfo{Blocking = true} },
    };


    public TileBase lol;
    private int SizeX = 10;
    private int SizeY = 10;

    Dictionary<TileType, TileBase> Tilebases = new Dictionary<TileType, TileBase>();
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
        if (Grid == null)
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
                var tileLayer = TileLayer.OBJECT;
                var tileType = GetTile(coord, tileLayer);
                var prefab = GetPrefabForTile(tileType);
                if (prefab == null)
                {
                    tileLayer = TileLayer.GROUND;
                    tileType = GetTile(coord, tileLayer);
                    prefab = GetPrefabForTile(tileType);
                }

                if (prefab == null)
                {
                    continue;
                }
                var newObject = Instantiate(prefab);
                Tilebases[tileType] = GetTileBase(coord, tileLayer);
                var position = GetWorldPosFromTile(coord);
                newObject.transform.position = position;
                var renderer = newObject.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 6;
                var draggable = newObject.GetComponent<Draggable>();
                if (draggable != null)
                {
                    draggable.AttachTile(tileType, coord, tileLayer);
                }
            }
        }
    }

    public TileType GetTile(TileCoord coord, TileLayer layer)
    {
        var vector = Utils.CoordToVector(coord);
        var tile = ((SuperTiled2Unity.SuperTile)Tilemaps[layer].GetTile(vector));
        if (tile == null)
        {
            return TileType.EMPTY;
        }
        var id = tile.m_TileId;
        return (TileType)id;
    }

    public TileBase GetTileBase(TileCoord coord, TileLayer layer)
    {
        var vector = Utils.CoordToVector(coord);
        return Tilemaps[layer].GetTile(vector);
    }

    public void SetTile(TileCoord pos, TileLayer layer, TileType tile)
    {
        var vector = Utils.CoordToVector(pos);
        var tileBase = Tilebases[tile];
        Tilemaps[layer].SetTile(vector, tileBase);
    }
    public void ClearTile(TileCoord pos, TileLayer layer)
    {
        var vector = Utils.CoordToVector(pos);
        Tilemaps[layer].SetTile(vector, null);
    }

    public Vector3 GetWorldPosFromTile(TileCoord coord)
    {
        return Grid.CellToWorld(Utils.CoordToVector(coord)) + new Vector3(Grid.cellSize.x, -Grid.cellSize.y) / 2;
    }

    public TileCoord GetTileCoordFromWorld(Vector3 pos)
    {
        var vec = Grid.WorldToCell(pos);
        return new TileCoord(vec.x, -vec.y - 1);
    }

    GameObject GetPrefabForTile(TileType tileType)
    {
        foreach (var t in TileObjectPrefabs)
        {
            if (t.tileType == tileType)
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
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int nX = f.X + i;
                int nY = f.Y + j;
                if (nX < -10 || nY < 0 || nX > 20 || nY >= 10)
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
                if (i != 0 && j != 0)
                {
                    var corner1 = new TileCoord(nX, f.Y);
                    var corner2 = new TileCoord(f.X, nY);
                    if (IsBlocking(corner1) || IsBlocking(corner2))
                    {
                        continue;
                    }
                    var worseCornerCost = Mathf.Max(GetDangerCost(corner1), GetDangerCost(corner2));
                    cost = Mathf.Sqrt(cost * cost + worseCornerCost * worseCornerCost);
                }
                result.Add(new NavNode(nCoord, cost));
            }
        }
        return result;
    }
}
