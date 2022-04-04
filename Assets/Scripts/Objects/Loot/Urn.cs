using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Urn : LootObject
{

    [SerializeField] Sprite[] spriteVariants;
    [SerializeField] SnakeHazard snakePrefab;
    [SerializeField] AudioClip snakeHissSFX;

    private bool hazardCleared;

    protected override void Awake()
    {
        Sprite pickedSprite = spriteVariants[Random.Range(0, spriteVariants.Length)];
        GetComponent<SpriteRenderer>().sprite = pickedSprite;
        nonLootedState = pickedSprite;
        lootedState = pickedSprite;
        hazardCleared = false;
    }

    private void OnMouseDown()
    {
        if (!hazardCleared)
        {
            hazardCleared = true;
            Instantiate<SnakeHazard>(snakePrefab, transform.position, Quaternion.identity, transform);
        }
    }

    public override void OnLoot(Adventurer adventurer)
    {
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
        if (!hazardCleared)
        {
            GetComponent<AudioSource>().PlayOneShot(snakeHissSFX);
            adventurer.Kill();
        }
    }
}
