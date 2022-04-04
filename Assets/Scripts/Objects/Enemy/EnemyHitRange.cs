using UnityEngine;

public class EnemyHitRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Adventurer")
        {
            Adventurer adventurer = other.gameObject.GetComponent<Adventurer>();
            if (adventurer != null)
            {
                transform.parent.GetComponent<Enemy>().Attack(adventurer, adventurer.transform.position.x < transform.position.x ? FACING_DIRECTION.LEFT : FACING_DIRECTION.RIGHT);
            }
        }
    }
}
