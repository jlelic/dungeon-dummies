using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource)), RequireComponent(typeof(SpriteRenderer))]
public class SnakeHazard : MonoBehaviour
{
    [SerializeField] float fadeDuration;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Fade());
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
