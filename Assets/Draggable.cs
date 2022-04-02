using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Shadowed Shadow;
    bool dragging = false;
    Vector3 LiftDistance = new Vector3(-0.15f, 0.3f, 0);
    Vector3 currentOffset;
    TileLayer TileLayer;
    TileCoord TileCoord;
    TileCoord newTileCoord;
    TileType TileType;
    bool hasAttachedTile = false;
    bool isValidPlacement = true;

    private void Start()
    {
        Shadow = GetComponent<Shadowed>();
    }

    public void AttachTile(TileType tileType, TileCoord tileCoord, TileLayer tileLayer)
    {
        hasAttachedTile = true;
        TileType = tileType;
        TileCoord = tileCoord;
        TileLayer = tileLayer;
    }

    private void Update()
    {
        if (dragging)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition = mouseWorldPosition - Vector3.forward * mouseWorldPosition.z;
            transform.position = mouseWorldPosition + currentOffset;
            isValidPlacement = UIManager.Instance.HighlightTile(mouseWorldPosition);
            newTileCoord = GridManager.Instance.GetTileCoordFromWorld(mouseWorldPosition);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("DRAG");
        currentOffset = Vector3.zero;
        dragging = true;
        if (hasAttachedTile)
        {
            GridManager.Instance.ClearTile(TileCoord, TileLayer);
        }
        Shadow.Unstick(LiftDistance);
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", Vector3.zero,
            "to", LiftDistance,
            "onupdate", (Action<Vector3>)((Vector3 value) => { currentOffset = value; })
            ));
    }

    private void OnMouseUp()
    {
        if (!dragging)
        {
            return;
        }
        UIManager.Instance.StopHighlight();
        Debug.Log("STOP DRAG");
        iTween.Stop(gameObject);
        dragging = false;
        var targetTileCoord = isValidPlacement ? newTileCoord : TileCoord;
        if(isValidPlacement)
        {
            TileCoord = newTileCoord;
        }
        var targetTileWorldPos = GridManager.Instance.GetWorldPosFromTile(targetTileCoord);
        Shadow.SetShadowPosition(targetTileWorldPos);
        Shadow.Stick();
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", Shadow.GetShadowPosition(),
            "time", 0.2f,
            "onfinished",(Action)(() => { GridManager.Instance.SetTile(TileCoord, TileLayer, TileType); })
        ));
    }

    void OnMouseEnter()
    {
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        //Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }

}
