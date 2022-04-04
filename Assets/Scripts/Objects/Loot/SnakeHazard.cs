using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SpriteRenderer))]
public class SnakeHazard : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float fadeDuration;

    private Vector3 direction;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        GetComponent<AudioSource>().Play();
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = RandomizeDirection();

        spriteRenderer.flipY = direction.x * direction.y > 0;
        float rotation = Vector2.SignedAngle(transform.position, new Vector2(direction.x, direction.y));
        transform.rotation = Quaternion.Euler(0, 0, -rotation);
        GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * speed;

        StartCoroutine(Fade());
    }

    private Vector2 RandomizeDirection()
    {
        return new Vector2(Random.Range(0.5f, 1f) * RandomSign(), Random.Range(0.5f, 1f) * RandomSign());
    }

    private float RandomSign()
    {
        if (Random.Range(0, 2) == 0)
        {
            return -1;
        }
        return 1;
    }

    public IEnumerator Fade()
    {
        float speed = (float)1f / fadeDuration;
        Color c = spriteRenderer.color;

        for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
        {
            c.a = Mathf.Lerp(1, 0, t);
            spriteRenderer.color = c;
            yield return true;
        }
        Destroy(gameObject);
    }
}
