using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] GameObject arrowSpawnPoint;

    private void Shoot()
    {
        GetComponent<AudioSource>().Play();
        Arrow arrow = Instantiate<Arrow>(arrowPrefab, transform.position, Quaternion.identity, transform);
        arrow.GetComponent<Rigidbody2D>().velocity = GetFireDirection().normalized * arrow.arrowSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Adventurer")
        {
            GetComponent<Animator>().SetTrigger("Shoot");
        }
    }

    private Vector2 GetFireDirection()
    {
        switch (facing)
        {
            case FACING_DIRECTION.LEFT:
                return Vector2.left;
            case FACING_DIRECTION.RIGHT:
            default:
                return Vector2.right;

        }
    }
}
