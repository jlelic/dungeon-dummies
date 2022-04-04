using UnityEngine;

public enum FACING_DIRECTION
{
    LEFT,
    RIGHT
}

[RequireComponent(typeof(Animator)), RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))]
public abstract class Enemy : MonoBehaviour
{
    public FACING_DIRECTION facing;

    protected bool active;

    protected virtual void Awake()
    {
        active = false;
        transform.Rotate(0f, 0f, facing == FACING_DIRECTION.LEFT ? 180f : 0f);
        GetComponent<SpriteRenderer>().flipY = facing == FACING_DIRECTION.LEFT;
    }


    public void Activate()
    {
        active = true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
