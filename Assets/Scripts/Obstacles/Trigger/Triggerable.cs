using UnityEngine;

public enum TRIGGER_DIRECTION
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
}
public abstract class Triggerable : MonoBehaviour
{
    public TRIGGER_DIRECTION direction;

    public abstract void Trigger();

    protected Quaternion GetFireRotation()
    {
        switch (direction)
        {
            case TRIGGER_DIRECTION.UP:
                return Quaternion.Euler(0f, 0f, 90f);
            case TRIGGER_DIRECTION.DOWN:
                return Quaternion.Euler(0f, 0f, -90f);
            case TRIGGER_DIRECTION.LEFT:
                return Quaternion.Euler(0f, 0f, 180f);
            case TRIGGER_DIRECTION.RIGHT:
            default:
                return Quaternion.identity;

        }
    }
}
