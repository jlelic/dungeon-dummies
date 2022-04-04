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
    private Adventurer adventurerToAttack;

    protected virtual void Awake()
    {
        active = false;
        transform.Rotate(0f, 0f, facing == FACING_DIRECTION.LEFT ? 180f : 0f);
        GetComponent<SpriteRenderer>().flipY = facing == FACING_DIRECTION.LEFT;
    }

    private void Start()
    {
        GridManager grid = GridManager.Instance;
        TileCoord coord = grid.GetTileCoordFromWorld(transform.position);
        GridManager.Instance.ClearTile(coord, TileLayer.OBJECT);
    }

    public void Activate()
    {
        active = true;
    }

    public void Die()
    {
        if (gameObject != null)
        {
            Instantiate(ObjectStore.Instance.BoneExplosionParticleEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Called by Attack Animation event 
    public void OnAttack()
    {
        GetComponent<AudioSource>().Play();
        adventurerToAttack.Kill();
    }

    public virtual void Attack(Adventurer adventurer, FACING_DIRECTION faceTo)
    {
        if (active)
        {
            adventurerToAttack = adventurer;
            if (faceTo != facing)
            {
                transform.Rotate(0f, 0f, faceTo == FACING_DIRECTION.LEFT ? 180f : 0f);
                GetComponent<SpriteRenderer>().flipY = faceTo == FACING_DIRECTION.LEFT;
                facing = faceTo;
            }
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }
}
