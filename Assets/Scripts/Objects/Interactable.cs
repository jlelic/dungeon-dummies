using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    SpriteRenderer interactableRender;
    protected bool CanPlayerInteract = true;

    protected virtual void Awake()
    {
        interactableRender = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (CanPlayerInteract && !LevelManager.Instance.IsPlaying)
        {
            interactableRender.color = Color.yellow;
        }
    }

    private void OnMouseExit()
    {
        if (CanPlayerInteract)
        {
            interactableRender.color = Color.white;
        }
    }
}
