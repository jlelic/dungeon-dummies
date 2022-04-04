using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuriousBehavior : Adventurer
{
    [ColorUsageAttribute(true, true)]
    public Color emissionColor;

    List<CuriousInterest> interests;
    protected override void Start()
    {
        base.Start();
        renderer.material.SetColor("_EmissionColor", emissionColor);
        interests = new List<CuriousInterest>(FindObjectsOfType<CuriousInterest>());
    }

    protected override Interest GetNextInterest()
    {
        var closest = navigation.GetClosestInterest(interests);
        if(closest == null)
        {
            return base.GetNextInterest();
        }
        return closest;
    }

    protected override void OnInterestedInteracted(Interest interest)
    {
        interests.Remove((CuriousInterest)interest);
    }
}
