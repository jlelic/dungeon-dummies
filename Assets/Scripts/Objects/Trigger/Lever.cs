using UnityEngine;

public class Lever : TriggerObject {
    new SpriteRenderer renderer;

    [SerializeField] Toggleable objectToToggle;

    protected virtual void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        objectToToggle.Toggle();
        onTriggerInteract();
    }


    private void OnMouseEnter()
    {
        if (!LevelManager.Instance.IsPlaying)
        {
            renderer.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        renderer.color = Color.white;
    }

    public override void Interact(Adventurer adventurer)
    {

        objectToToggle.Toggle();
        onTriggerInteract();
    }

    //public override void Interact(Adventurer adventurer)
    //{
    //    onTriggerInteract();
    //}

    //private void onTriggerInteract()
    //{
    //    GetComponent<SpriteRenderer>().sprite = onState;
    //    GetComponent<AudioSource>().Play();
    //    Invoke("WaitForTrigger", 0.5f);
    //}

    //private void WaitForTrigger()
    //{
    //    objectToTrigger.Trigger();
    //}
}
