using UnityEngine;

public class Antidote : LootObject
{
    [SerializeField] AudioClip drinkingSFX;

    private void Start()
    {
        GridManager grid = GridManager.Instance;
        TileCoord coord = grid.GetTileCoordFromWorld(transform.position);
        GridManager.Instance.ClearTile(coord, TileLayer.OBJECT);
    }

    public override void BeginInteract()
    {
        GetComponent<AudioSource>().PlayOneShot(drinkingSFX);
    }

    public override void OnLoot(Adventurer adventurer)
    {
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
        adventurer.isPoisonImmune = true;
    }
}
