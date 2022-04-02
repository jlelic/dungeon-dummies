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

    public string ToString()
    {
        return X + "x" + Y;
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
}


public class GridManager : MonoBehaviour
{
    static GridManager Instance;

    static public Dictionary<TileType, TileInfo> TileInfo = new Dictionary<TileType, TileInfo>()
    {
        {TileType.LAVA, new TileInfo{Burning = true } },
        {TileType.GROUND, new TileInfo{} },
        {TileType.BRIDGE, new TileInfo{} },
        {TileType.WALL, new TileInfo{Blocking = true} }
    };


    public TileBase lol;
    public int SizeX = 0;
    public int SizeY = 0;

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
    void Start()
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
            else if (name == "bridge")
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
            SizeX = Mathf.Max(map.size.x);
            SizeY = Mathf.Max(map.size.y);
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
                var tileType = GetTile(coord, TileLayer.OBJECT);
                if (tileType == TileType.EMPTY)
                {
                    continue;
                }
                var prefab = GetPrefabForTile(tileType);
                if (prefab == null)
                {
                    continue;
                }
                Instantiate(prefab);
                var position = GetWorldPosFromTile(coord);
                Debug.Log(tileType + " at " + coord.ToString());
                prefab.transform.position = position;
                var renderer = prefab.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 4;
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

    public bool IsBlockingTile(TileCoord pos)
    {
        var result = false;
        return result;
    }

    public Vector3 GetWorldPosFromTile(TileCoord coord)
    {
        return Grid.CellToWorld(Utils.CoordToVector(coord)) + new Vector3(Grid.cellSize.x, -Grid.cellSize.y)/2;
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
}
