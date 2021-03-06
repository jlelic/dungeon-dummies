using UnityEngine;

public class Urn : LootObject
{

    [SerializeField] SnakeHazard snakePrefab;
    [SerializeField] Sprite[] spriteVariants;

    protected override void Awake()
    {
        Sprite pickedSprite = spriteVariants[Random.Range(0, spriteVariants.Length)];
        GetComponent<SpriteRenderer>().sprite = pickedSprite;
        nonLootedState = pickedSprite;
        lootedState = pickedSprite;
    }

    public override void OnLoot(Adventurer adventurer)
    {
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
        Instantiate<SnakeHazard>(snakePrefab, transform.position, Quaternion.identity, transform);
        adventurer.Poison();
    }
}
