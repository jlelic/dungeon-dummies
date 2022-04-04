using UnityEngine;

public class MeleeEnemy : Enemy
{

    private Adventurer adventurerToAttack;

    // Called by Attack Animation event 
    public void OnAttack()
    {
        GetComponent<AudioSource>().Play();
        adventurerToAttack.Kill();
    }

    public void Attack(Adventurer adventurer, FACING_DIRECTION faceTo)
    {
        if (active)
        {
            adventurerToAttack = adventurer;
            if (faceTo != facing)
            {
                transform.Rotate(0f, 0f, faceTo == FACING_DIRECTION.LEFT ? 180f : 0f);
                GetComponent<SpriteRenderer>().flipY = faceTo == FACING_DIRECTION.LEFT;
                facing = faceTo;
            }
            GetComponent<Animator>().SetTrigger("Attack");
        }
    }
}
