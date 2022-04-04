using UnityEngine;

public class Lever : TriggerObject {
    new SpriteRenderer renderer;

    protected virtual void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
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
}
