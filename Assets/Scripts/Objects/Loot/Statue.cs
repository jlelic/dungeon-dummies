using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Statue : LootObject
{
    [SerializeField] new Light2D light;

    private void Start()
    {
        Debug.Log(Coord);
    }

    public override void OnLoot()
    {
        Destroy(light.gameObject);
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
    }
}


