using System.Collections;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] GameObject arrowSpawnPoint;

    private bool canShoot = true;

    // Called by Shoot Animation event 
    private void Shoot()
    {
        GetComponent<AudioSource>().Play();
        Arrow arrow = Instantiate<Arrow>(arrowPrefab, transform.position, Quaternion.Euler(0f, 0f, facing == FACING_DIRECTION.LEFT ? 180f : 0f), transform);
        arrow.GetComponent<Rigidbody2D>().velocity = GetFireDirection().normalized * arrow.arrowSpeed;
    }

    private void Update()
    {
        if (canShoot && active)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, facing == FACING_DIRECTION.LEFT ? Vector2.left : Vector2.right, 100f, LayerMask.GetMask("Adventurer"));
            if (hit.collider != null)
            {
                Adventurer adventurer = hit.transform.gameObject.GetComponent<Adventurer>();
                if (adventurer != null)
                {
                    GetComponent<Animator>().SetTrigger("Shoot");
                    canShoot = false;
                    StartCoroutine(ShootDelay());
                }

            }
        }
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(2.5f);
        canShoot = true;
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
