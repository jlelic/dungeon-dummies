using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Statue : LootObject
{
    [SerializeField] new Light2D light;

    public override void OnLoot(Adventurer adventurer)
    {
        Destroy(light.gameObject);
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
    }
}


