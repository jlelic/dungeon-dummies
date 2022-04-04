using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    public float arrowSpeed;

    private void Start()
    {
        Destroy(gameObject, 10f);
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
