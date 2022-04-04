using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHazard : MonoBehaviour
{
    protected int CoveredUp = 0;

    public bool IsCovered { get { return CoveredUp > 0; } }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        var boulder = collision.gameObject.GetComponent<Boulder>();
        int wasCoveredUp = CoveredUp;
        if(boulder != null && CoveredUp == 0)
        {
            CoveredUp++;
            boulder.FallIntoHole(this);
            GridManager.Instance.NeutralizeTile(GridManager.Instance.GetTileCoordFromWorld(transform.position));
        }
        if (collision.gameObject.tag == "Platform")
        {
            CoveredUp++;
        }
        if(CoveredUp == 1 && wasCoveredUp == 0)
        {
            Navigation.RecalculateAll();
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            CoveredUp--;
        }
    }
}
