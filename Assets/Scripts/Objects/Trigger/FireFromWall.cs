using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireFromWall : MonoBehaviour
{

    private void Start()
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 1f);
    }
}
