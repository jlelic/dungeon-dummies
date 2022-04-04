using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(Collider2D))]
public class WallFireTrap : Toggleable
{

    [SerializeField] FireFromWall firePrefab;
    [SerializeField] Sprite disabledState;
    new SpriteRenderer renderer;
    Sprite enabledState;


   [SerializeField] bool trapEnabled;

    protected override void Awake()
    {
        base.Awake();
        CanPlayerInteract = false;
        trapEnabled = true;
        renderer = GetComponent<SpriteRenderer>();
        enabledState = renderer.sprite;
    }

    public override void Trigger()
    {
        if (trapEnabled)
        {
            Instantiate<FireFromWall>(firePrefab, transform.position, GetFireRotation(), transform);
        }
    }

    public override void Toggle()
    {
        trapEnabled = !trapEnabled;
        GetComponent<AudioSource>().Play();
        renderer.sprite = trapEnabled ? enabledState : disabledState;
        GetComponent<Animator>().SetBool("doused", !trapEnabled);
    }
}
