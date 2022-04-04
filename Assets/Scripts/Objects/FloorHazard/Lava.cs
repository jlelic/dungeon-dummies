using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Lava : FloorHazard
{
    new Light2D light;
    float lightIntensity;
    float lightRadius;

    private void Start()
    {
        light = GetComponent<Light2D>();
        lightIntensity = light.intensity;
        lightRadius = light.pointLightOuterRadius;
    }


    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        light.enabled = !IsCovered;
        if (IsCovered)
        {
            return;
        }

        Action<float> onLightUpdate = (float value) =>
        {
            light.intensity = lightIntensity * value;
            light.pointLightOuterRadius = lightRadius * value;
        };

        var adventurer = collision.gameObject.GetComponent<Adventurer>();
        if (adventurer != null)
        {
            adventurer.Burn();
            iTween.Stop(gameObject);
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 1,
                "to", 2,
                "time", 0.6f,
                "easetype", iTween.EaseType.easeInBounce,
                "onupdate", onLightUpdate
                ));
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", 2,
                "to", 1,
                "delay", .65f,
                "time", 0.2f,
                "easetype", iTween.EaseType.easeOutQuad,
                "onupdate", onLightUpdate
                ));
        }
    }



    override protected void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        light.enabled = !IsCovered;
    }
}
