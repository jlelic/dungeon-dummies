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
                transform.parent.GetComponent<MeleeEnemy>().Attack(adventurer);
            }
        }
    }
}
