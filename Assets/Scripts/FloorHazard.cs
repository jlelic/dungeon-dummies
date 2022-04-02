using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorHazard : MonoBehaviour
{
    protected int CoveredUp = 0;

    public bool IsCovered { get { return CoveredUp > 0; } }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            CoveredUp++;
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
