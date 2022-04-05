using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SpriteRenderer))]
public abstract class TriggerObject : CuriousInterest
{
    [SerializeField] Triggerable objectToTrigger;
    [SerializeField] protected Sprite offState;
    [SerializeField] protected Sprite onState;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = offState;
    }

    public override void Interact(Adventurer adventurer)
    {
        onTriggerInteract();
    }

    protected void onTriggerInteract()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteRenderer.sprite.name == onState.name ? offState : onState;
        GetComponent<AudioSource>().Play();
        Invoke("WaitForTrigger", 0.5f);
    }

    private void WaitForTrigger()
    {
        if (objectToTrigger != null) {
            objectToTrigger.Trigger();
        }
    }
}
