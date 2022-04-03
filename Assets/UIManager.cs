using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager Instance;

    [SerializeField] SpriteRenderer GridHighlight;
    [SerializeField] Sprite OkGridHighlightSprite;
    [SerializeField] Sprite NoGridHighlightSprite;

    GridManager Grid;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate singleton UI Manager found");
            Destroy(this);
        }
        StopHighlight();
    }

    private void Start()
    {
        Grid = GridManager.Instance;
    }

    public bool HighlightTile(Vector3 position)
    {
        GridHighlight.gameObject.SetActive(true);
        var coord = Grid.GetTileCoordFromWorld(position);
        var objectTile = Grid.GetTile(coord, TileLayer.OBJECT);
        bool validPlacement = !Grid.IsBlocking(coord) && objectTile.Equals(TileType.EMPTY);

        if (validPlacement)
        {
            GridHighlight.sprite = OkGridHighlightSprite;
        }
        else
        {
            GridHighlight.sprite = NoGridHighlightSprite;
        }
        var tileWorldPos = Grid.GetWorldPosFromTile(coord);
        GridHighlight.transform.position = tileWorldPos;
        return validPlacement;
    }

    public void StopHighlight()
    {
        GridHighlight.gameObject.SetActive(false);
    }
    
}
