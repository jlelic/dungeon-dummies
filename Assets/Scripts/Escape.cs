using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : Interest
{
    TileCoord coord;
    GridManager Grid;
    HashSet<string> escaped;
    // Start is called before the first frame update
    void Start()
    {
        escaped = new HashSet<string>();
        InteractableDistance = 0;
        Grid = GridManager.Instance;
        coord = Grid.GetTileCoordFromWorld(transform.position);
        GridManager.Instance.ClearTile(coord, TileLayer.OBJECT);
        LevelManager.Instance.RegisterEscape(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Adventurer")
        {
            var name = collision.gameObject.name;
            if(escaped.Contains(name))
            {
                return;
            }
            escaped.Add(name);
            LevelManager.Instance.OnAdventurerEscaped();
            var navigation = collision.GetComponent<Navigation>();
            navigation.Stop();
            iTween.MoveTo(collision.gameObject, Grid.GetWorldPosFromTile(new TileCoord(coord.X + 2, coord.Y)), 5);
        }
    }

    public override void Interact(Adventurer adventurer)
    {
    }
}
