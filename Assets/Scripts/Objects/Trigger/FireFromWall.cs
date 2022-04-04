using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Collider2D))]
public class FireFromWall : MonoBehaviour
{

    private void Start()
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Adventurer")
        {
            other.gameObject.GetComponent<Adventurer>().Burn();
        }
    }
}
