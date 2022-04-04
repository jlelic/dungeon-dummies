using UnityEngine;

public class Urn : LootObject
{

    [SerializeField] Sprite[] spriteVariants;
    [SerializeField] AudioClip snakeHissSFX;

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
        GetComponent<AudioSource>().PlayOneShot(snakeHissSFX);
        adventurer.Poison();
    }
}
