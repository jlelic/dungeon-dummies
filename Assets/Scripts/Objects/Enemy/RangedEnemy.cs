using System.Collections;
using UnityEngine;

public enum SHOOT_DIRECTION
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class RangedEnemy : Enemy
{
    [SerializeField] Arrow arrowPrefab;
    [SerializeField] GameObject arrowSpawnPoint;
    [SerializeField] SHOOT_DIRECTION shootDirection;
    [SerializeField] AudioClip shootSFX;

    private bool canShoot = true;

    private void Update()
    {
        if (canShoot && active)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetFireDirection(), 100f, LayerMask.GetMask("Adventurer"));
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

    // Called by Shoot Animation event 
    private void Shoot()
    {
        GetComponent<AudioSource>().PlayOneShot(shootSFX);
        Arrow arrow = Instantiate<Arrow>(arrowPrefab, transform.position, GetFireRotation(), transform);
        arrow.GetComponent<Rigidbody2D>().velocity = GetFireDirection().normalized * arrow.arrowSpeed;
    }

    public override void Attack(Adventurer adventurer, FACING_DIRECTION faceTo)
    {
        base.Attack(adventurer, faceTo);

        StopCoroutine(ShootDelay());
        canShoot = false;
        StartCoroutine(ShootDelay());
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(2.5f);
        canShoot = true;
    }

    private Vector2 GetFireDirection()
    {
        switch (shootDirection)
        {
            case SHOOT_DIRECTION.UP:
                return Vector2.up;
            case SHOOT_DIRECTION.DOWN:
                return Vector2.down;
            case SHOOT_DIRECTION.LEFT:
                return Vector2.left;
            case SHOOT_DIRECTION.RIGHT:
            default:
                return Vector2.right;

        }
    }

    private Quaternion GetFireRotation()
    {
        switch (shootDirection)
        {
            case SHOOT_DIRECTION.UP:
                return Quaternion.Euler(0f, 0f, 90f);
            case SHOOT_DIRECTION.DOWN:
                return Quaternion.Euler(0f, 0f, -90f);
            case SHOOT_DIRECTION.LEFT:
                return Quaternion.Euler(0f, 0f, 180f);
            case SHOOT_DIRECTION.RIGHT:
            default:
                return Quaternion.identity;

        }
    }
}
