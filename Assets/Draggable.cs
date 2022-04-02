using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Shadowed Shadow;
    bool dragging = false;

    private void Start()
    {
        Shadow = GetComponent<Shadowed>();
    }

    private void Update()
    {
        if(dragging)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition = mouseWorldPosition - Vector3.forward * mouseWorldPosition.z;
            transform.position = mouseWorldPosition;
        }
    }

    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    void OnMouseEnter()
    {
        Debug.Log("Enter");
        Shadow.Unstick();
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        //Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        Shadow.Stick();
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }

}
