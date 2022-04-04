using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadowed : MonoBehaviour
{

    GameObject Shadow;
    bool sticking = true;
    Vector3 stickWorldPosition;

    void Start()
    {
        Shadow = GameObject.Instantiate(new GameObject());
        Shadow.name = "Shadow of " + gameObject.name;
        var shadowRenderer = Shadow.AddComponent<SpriteRenderer>();
        var renderer = GetComponent<SpriteRenderer>();
        shadowRenderer.sprite = renderer.sprite;
        shadowRenderer.color = new Color(0,0,0,0.8f);
        shadowRenderer.sortingOrder = renderer.sortingOrder - 1;
        Shadow.transform.SetParent(transform);
        Shadow.transform.localPosition = Vector3.zero;

        stickWorldPosition = transform.position;
    }

    public void Unstick(Vector3 distance)
    {
        sticking = false;
        iTween.Stop(Shadow);
        iTween.MoveBy(Shadow, iTween.Hash(
            "amount", -distance,
            "time", 0.5f
            ));
    }

    public void Stick()
    {
        sticking = true;
        stickWorldPosition = Shadow.transform.position;
    }

    private void Update()
    {
        if(sticking)
        {
            Shadow.transform.position = stickWorldPosition;
        }
    }

    internal void SetShadowPosition(Vector3 position)
    {
        Shadow.transform.position = position;
    }

    internal Vector3 GetShadowPosition()
    {
        return Shadow.transform.position;
    }
}
