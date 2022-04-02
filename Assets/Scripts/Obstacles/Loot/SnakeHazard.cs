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
        StartCoroutine(Fade());
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
        float rotation = Vector2.SignedAngle(Vector2.left, new Vector2(direction.x, direction.y));
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private Vector2 RandomizeDirection()
    {
        return new Vector2(Random.Range(0, 1f), Random.Range(0, 1f));
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
