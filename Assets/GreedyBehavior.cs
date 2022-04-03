using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBehavior : Adventurer
{
    List<GreedyInterest> interests;
    protected override void Start()
    {
        base.Start();
        interests = new List<GreedyInterest>(FindObjectsOfType<GreedyInterest>());
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
        interests.Remove((GreedyInterest)interest);
    }
}
