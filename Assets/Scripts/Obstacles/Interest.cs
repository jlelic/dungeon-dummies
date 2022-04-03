using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interest : MonoBehaviour
{
    public TileCoord Coord { get { return GridManager.Instance.GetTileCoordFromWorld(transform.position); } }
    public float InteractableDistance { get; protected set; } = 1;
    public bool IsActive { get; protected set; }

    public bool CanInteractFrom(Vector3 from)
    {
        return Vector3.Distance(transform.position, from) <= InteractableDistance;
    }

    public abstract void Interact(Adventurer adventurer);

}
