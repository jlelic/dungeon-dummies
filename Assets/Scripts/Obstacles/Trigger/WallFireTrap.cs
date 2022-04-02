using UnityEngine;


[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(Collider2D))]
public class WallFireTrap : Triggerable
{

    [SerializeField] FireFromWall firePrefab;
    [SerializeField] Sprite disabledState;

    private bool trapEnabled;

    private void Awake()
    {
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
    }
}
