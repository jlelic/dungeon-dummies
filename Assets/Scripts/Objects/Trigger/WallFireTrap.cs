using UnityEngine;


[RequireComponent(typeof(Animator)), RequireComponent(typeof(Collider2D))]
public class WallFireTrap : Triggerable
{

    [SerializeField] FireFromWall firePrefab;
    [SerializeField] Sprite disabledState;

    private bool trapEnabled;

    protected override void Awake()
    {
        base.Awake();
        trapEnabled = true;
    }

    public override void Trigger()
    {
        if (trapEnabled)
        {
            Instantiate<FireFromWall>(firePrefab, transform.position, GetFireRotation(), transform);
        }
    }

    private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().sprite = disabledState;
        trapEnabled = false;
        GetComponent<Animator>().SetBool("doused", true);
    }
}
