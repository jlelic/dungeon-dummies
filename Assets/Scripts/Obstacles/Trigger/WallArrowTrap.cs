using UnityEngine;

public class WallArrowTrap : Triggerable
{

    [SerializeField] Arrow arrowPrefab;

    public override void Trigger()
    {
        GetComponent<AudioSource>().Play();
        Arrow arrow = Instantiate<Arrow>(arrowPrefab, transform.position, GetFireRotation(), transform);
        arrow.GetComponent<Rigidbody2D>().velocity = GetFireDirection().normalized * arrow.arrowSpeed;
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
