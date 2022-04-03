using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] bool IsToggleType;
    bool IsToggled;
    [SerializeField] Sprite SpriteClicked;
    Sprite spriteOriginal;
    new SpriteRenderer renderer;
    Vector3 originalPosition;
    List<Action> listeners;

    void Start()
    {
        Debug.Log("START " + gameObject.name);
        originalPosition = transform.position;
        renderer = GetComponent<SpriteRenderer>();
        spriteOriginal = renderer.sprite;
        if (gameObject.name == "ButtonPlay")
        {
            renderer.material.SetVector("_EmissionColor", Vector4.zero);
        }
    }

    public void AddOnClickListener(Action listener)
    {
        if (listeners == null)
        {
            listeners = new List<Action>();
        }
        listeners.Add(listener);
    }

    private void OnMouseEnter()
    {
        if (IsToggleType && IsToggled)
        {
            return;
        }
        transform.position = originalPosition + Vector3.up / 16;
    }

    private void OnMouseExit()
    {
        if (IsToggleType && IsToggled)
        {
            return;
        }
        transform.position = originalPosition;
        renderer.sprite = spriteOriginal;
    }

    private void OnMouseDown()
    {
        transform.position = originalPosition;
        renderer.sprite = SpriteClicked;
    }

    private void OnMouseUp()
    {
        renderer.sprite = spriteOriginal;

        if (IsToggleType && IsToggled)
        {
            return;
        }

        if (IsToggleType)
        {
            if (IsToggled)
            {
                return;
            }
            else
            {
                IsToggled = true;
            }
        }
        if (listeners == null)
        {
            return;
        }

        foreach (var l in listeners)
        {
            l();
        }

    }

    public void LightUp(float time)
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0,
            "to", 1f,
            "time", time,
            "looptype", iTween.LoopType.pingPong,
            "onupdate", (Action<float>)((value) => { var x = value * 5; renderer.material.SetVector("_EmissionColor", new Vector4(x/2,x/2,x,x)); })
            ));
    }

    public void Toggle()
    {
        IsToggled = true;
        renderer.sprite = SpriteClicked;
    }
    public void Untoggle()
    {
        IsToggled = false;
        renderer.sprite = spriteOriginal;
    }
}
