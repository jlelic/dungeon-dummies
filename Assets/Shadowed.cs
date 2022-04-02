using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadowed : MonoBehaviour
{

    Vector3 Distance = new Vector3(-0.2f,-0.5f,0);
    GameObject Shadow;

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
    }

    public void Unstick()
    {
        iTween.Stop(Shadow);
        iTween.MoveBy(Shadow, iTween.Hash(
            "amount", Distance,
            "time", 0.5f
            ));
    }

    public void Stick()
    {
        iTween.Stop(Shadow);
        iTween.MoveTo(Shadow, iTween.Hash(
            "position", transform.position,
            "time", 0.5f
            ));
    }
}
