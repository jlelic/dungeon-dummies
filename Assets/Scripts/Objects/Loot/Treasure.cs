public class Treasure : LootObject
{
    public override void OnLoot()
    {
        if (objectToTrigger)
        {
            objectToTrigger.Trigger();
        }
    }
}
