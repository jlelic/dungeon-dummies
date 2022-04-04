using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(Collider2D))]
public class PressurePlate : MonoBehaviour
{
    [SerializeField] Triggerable objectToTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Adventurer")
        {
            GetComponent<AudioSource>().Play();
            Invoke("WaitForTrigger", 0.5f);
        }
    }

    private void WaitForTrigger()
    {
        objectToTrigger.Trigger();
    }
}

