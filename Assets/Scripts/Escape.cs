using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    TileCoord coord;
    GridManager Grid;
    // Start is called before the first frame update
    void Start()
    {
        Grid = GridManager.Instance;
        coord = Grid.GetTileCoordFromWorld(transform.position);
        GridManager.Instance.ClearTile(coord, TileLayer.OBJECT);
        LevelManager.Instance.RegisterEscape(coord);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Adventurer")
        {
            LevelManager.Instance.OnAdventurerEscaped();
            var navigation = collision.GetComponent<Navigation>();
            iTween.MoveTo(collision.gameObject, Grid.GetWorldPosFromTile(new TileCoord(coord.X + 2, coord.Y)), 5);
        }
    }
}
