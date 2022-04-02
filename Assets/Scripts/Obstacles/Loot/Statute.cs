public class Statute : LootObject
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


