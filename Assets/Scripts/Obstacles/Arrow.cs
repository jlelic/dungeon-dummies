using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    public float arrowSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Adventurer")
        {
            Destroy(gameObject);
        }
    }
}
