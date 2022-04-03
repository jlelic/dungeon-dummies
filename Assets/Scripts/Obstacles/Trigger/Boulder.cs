using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))]
public class Boulder : MonoBehaviour
{
    [SerializeField] AudioClip impactSFX;
    public float boulderSpeed;
    public TRIGGER_DIRECTION boulderDirection { private get; set; }

    private Vector3 endDestination;
    private bool boulderStopped;

    private void Start()
    {
        boulderStopped = false;
    }
    private void Update()
    {
        if (!boulderStopped)
        {
            TileCoord currentTileCoord = GridManager.Instance.GetTileCoordFromWorld(transform.position);
            TileCoord nextTileCoord = GetNextGridTileCoord(currentTileCoord);
            if (GridManager.Instance.IsBlocking(nextTileCoord))
            {
                endDestination = GridManager.Instance.GetWorldPosFromTile(currentTileCoord);
            }
            if (transform.position == endDestination)
            {
                StopBoulder();
            }
        }
    }

    private void StopBoulder()
    {
        boulderStopped = true;
        GetComponent<Animator>().SetBool("isRolling", false);
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(impactSFX);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }


    private TileCoord GetNextGridTileCoord(TileCoord currentCoord)
    {
        if (boulderDirection == TRIGGER_DIRECTION.LEFT)
        {
            return new TileCoord(currentCoord.X - 1, currentCoord.Y);
        }
        else if (boulderDirection == TRIGGER_DIRECTION.RIGHT)
        {
            return new TileCoord(currentCoord.X + 1, currentCoord.Y);
        }
        else if (boulderDirection == TRIGGER_DIRECTION.DOWN)
        {
            return new TileCoord(currentCoord.X, currentCoord.Y + 1);
        }
        else if (boulderDirection == TRIGGER_DIRECTION.UP)
        {
            return new TileCoord(currentCoord.X, currentCoord.Y - 1);

        }
        return currentCoord;
    }
}
