using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] GameObject arrowSpawnPoint;

    private bool didShoot = false;

    private void Shoot()
    {
        GetComponent<AudioSource>().Play();
        Arrow arrow = Instantiate<Arrow>(arrowPrefab, transform.position, Quaternion.identity, transform);
        arrow.GetComponent<Rigidbody2D>().velocity = GetFireDirection().normalized * arrow.arrowSpeed;
    }

    private void Update()
    {
        if (!didShoot)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 100f, LayerMask.GetMask("Adventurer"));
            if (hit.collider != null)
            {
                Adventurer adventurer = hit.transform.gameObject.GetComponent<Adventurer>();
                if (adventurer != null)
                {
                    didShoot = true;
                    GetComponent<Animator>().SetTrigger("Shoot");
                }

            }
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
