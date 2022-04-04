using UnityEngine;

public class WallBoulderTrap : Triggerable
{
    [SerializeField] Boulder boulderPrefab;
    bool alreadyFired = false;
    private void Start()
    {
        CanPlayerInteract = false;
    }

    public override void Trigger()
    {
        if(alreadyFired)
        {
            return;
        }
        alreadyFired = true;
        GetComponent<AudioSource>().Play();
        Invoke("InstantiateBoulder", 0.25f);
    }

    private void InstantiateBoulder()
    {
        Boulder boulder = Instantiate<Boulder>(boulderPrefab, transform.position, GetFireRotation(), transform);
        Vector2 fireDirection = GetFireDirection();
        if (fireDirection == Vector2.left)
        {
            boulder.GetComponent<SpriteRenderer>().flipY = true;
        }
        boulder.GetComponent<Rigidbody2D>().velocity = fireDirection.normalized * boulder.boulderSpeed;
        boulder.boulderDirection = direction;
    }

    private Vector2 GetFireDirection()
    {
        switch (direction)
        {
            case TRIGGER_DIRECTION.UP:
                return Vector2.up;
            case TRIGGER_DIRECTION.DOWN:
                return Vector2.down;
            case TRIGGER_DIRECTION.LEFT:
                return Vector2.left;
            case TRIGGER_DIRECTION.RIGHT:
            default:
                return Vector2.right;
        }
    }
}
