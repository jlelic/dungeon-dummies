using UnityEngine;

public class Treasure : LootObject
{
    [SerializeField] Sprite openState;
    [SerializeField] AudioClip openSFX;

    protected override void Awake()
    {
        base.Awake();
        TimeToInteract = 200;
    }

    public override void OnLoot(Adventurer adventurer)
    {
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
    }

    public override void BeginInteract()
    {
        Invoke("OpenTreasure", 0.5f);
    }

    private void OpenTreasure()
    {
        GetComponent<AudioSource>().PlayOneShot(openSFX);
        GetComponent<SpriteRenderer>().sprite = openState;
    }
}
