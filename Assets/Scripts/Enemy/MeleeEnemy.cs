using UnityEngine;

public class MeleeEnemy : Enemy
{

    private Adventurer adventurerToAttack;


    public void OnAttack()
    {
        GetComponent<AudioSource>().Play();
        adventurerToAttack.Kill();
    }

    public void Attack(Adventurer adventurer)
    {
        adventurerToAttack = adventurer;
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
