using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SpriteRenderer))]
public abstract class LootObject : GreedyInterest
{
    [Tooltip("Optional for Lootable objects")]
    public Triggerable objectToTrigger;

    [Tooltip("No effect on Urn")]
    public Sprite nonLootedState;
    [Tooltip("No effect on Urn")]
    public Sprite lootedState;

    protected virtual void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = nonLootedState;
    }

    public override void Interact(Adventurer adventurer)
    {
        GetComponent<SpriteRenderer>().sprite = lootedState;
        GetComponent<AudioSource>().Play();
        OnLoot(adventurer);
        IsActive = false;
    }

    public abstract void OnLoot(Adventurer adventurer);

}
