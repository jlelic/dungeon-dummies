using UnityEngine;

public class Pit : FloorHazard
{
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (IsCovered)
        {
            return;
        }
        var adventurer = collision.gameObject.GetComponent<Adventurer>();
        if (adventurer != null)
        {
            adventurer.FallToPit();
        }
    }



    override protected void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
