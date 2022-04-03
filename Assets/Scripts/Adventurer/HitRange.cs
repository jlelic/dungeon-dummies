using UnityEngine;

public class HitRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                transform.parent.GetComponent<BrusherBehavior>().Attack(enemy);
            }
        }
    }
}
