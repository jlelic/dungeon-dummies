using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    TileCoord originalCoord;

    private void Start()
    {
        originalCoord = GridManager.Instance.GetTileCoordFromWorld(transform.position);
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        var grid = GridManager.Instance;
        var coord = grid.GetTileCoordFromWorld(transform.position);
        if (!coord.Equals(originalCoord) && grid.IsBlocking(coord))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Adventurer")
        {
            Adventurer adventurer = other.gameObject.GetComponent<Adventurer>();
            adventurer.PierceByArrow();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Platform")
        {
            Pillar pillar = other.gameObject.GetComponent<Pillar>();
            if (pillar)
            {
                Destroy(gameObject);
            }
        }
    }
}
