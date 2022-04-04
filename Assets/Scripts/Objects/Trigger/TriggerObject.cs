using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SpriteRenderer))]
public abstract class TriggerObject : CuriousInterest
{
    [SerializeField] Triggerable objectToTrigger;
    [SerializeField] Sprite offState;
    [SerializeField] Sprite onState;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = offState;
    }

    public override void Interact(Adventurer adventurer)
    {
        onTriggerInteract();
    }

    private void onTriggerInteract()
    {
        GetComponent<SpriteRenderer>().sprite = onState;
        GetComponent<AudioSource>().Play();
        Invoke("WaitForTrigger", 0.5f);
    }

    private void WaitForTrigger()
    {
        objectToTrigger.Trigger();
    }
}
