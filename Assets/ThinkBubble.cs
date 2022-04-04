using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] SpriteRenderer Icon1;
    [SerializeField] SpriteRenderer Icon2;
    new SpriteRenderer renderer;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        Icon1.gameObject.SetActive(false);
        Icon2.gameObject.SetActive(false);
    }

    public void Think(Interest interest)
    {
        Icon1.sprite = interest.GetIcon();
        renderer.enabled = true;
        animator.SetTrigger("Think");
    }

    public void OnBubbleShow()
    {
        Icon1.gameObject.SetActive(true);
        Icon2.gameObject.SetActive(true);
    }

    public void OnBubbleHide()
    {
        Icon1.gameObject.SetActive(false);
        Icon2.gameObject.SetActive(false);
    }

    public void OnThinkEnd()
    {
        renderer.enabled = false;
    }
}
