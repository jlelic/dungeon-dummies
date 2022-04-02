public class Treasure : LootObject
{
    public override bool IsHazard()
    {
        return false;
    }

    public override void OnLoot()
    {
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
    }
}
