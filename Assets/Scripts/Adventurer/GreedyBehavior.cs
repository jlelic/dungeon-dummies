using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBehavior : Adventurer
{
    struct InterestInfo
    {
        public GreedyInterest interest;
        public bool seen;
        public bool completed;
    }

    [SerializeField] ThinkBubble thinkBubble;

    private List<InterestInfo> interests;

    protected override void Start()
    {
        base.Start();

        interests = new List<InterestInfo>();
        var greedyInterests = FindObjectsOfType<GreedyInterest>();
        foreach (var i in greedyInterests)
        {
            interests.Add(new InterestInfo { interest = i });
        }
    }

    public override void Poison()
    {
        if (!isPoisonImmune)
        {
            Kill();
        }
    }

    protected override Interest GetNextInterest()
    {
        var seenInterests = interests.FindAll(i => i.seen && !i.completed).ConvertAll<Interest>(i => i.interest);
        var closest = navigation.GetClosestInterest(seenInterests);
        if (closest == null)
        {
            return base.GetNextInterest();
        }
        return closest;
    }

    protected override void AIUpdate()
    {
        if (State == AdventurerState.Interacting)
        {
            return;
        }
        bool foundSomething = false;
        for (int i = 0; i < interests.Count; i++)
        {
            var info = interests[i];
            if (!info.seen && !info.completed)
            {
                if (CanSee(info.interest))
                {
                    navigation.Stop();
                    interests[i] = new InterestInfo { interest = info.interest, seen = true };
                    State = AdventurerState.Idle;
                    if (!foundSomething)
                    {
                        thinkBubble.Think(info.interest);
                    }
                    foundSomething = true;
                }
            }
        }
    }

    protected override void OnInterestedInteracted(Interest interest)
    {
        var index = interests.FindIndex(i => i.interest == interest);
        interests[index] = new InterestInfo { interest = (GreedyInterest)interest, seen = true, completed = true };
    }
}
