using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lava : FloorHazard
{
    new Light2D light;

    private void Start()
    {
        light = GetComponent<Light2D>();
    }


    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        light.enabled = !IsCovered;
        if(IsCovered)
        {
            return;
        }

        var adventurer = collision.gameObject.GetComponent<Adventurer>();
        if (adventurer != null)
        {
            adventurer.Burn();
        }
        Debug.Log(collision.gameObject.tag);
    }

    override protected void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        light.enabled = !IsCovered;
    }
}
