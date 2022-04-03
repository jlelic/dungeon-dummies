using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class BrusherBehavior : Adventurer
{
    private const float TIME_TO_ATTACK = 1.25f;

    protected override void Start()
    {
        base.Start();
    }

    public override void PierceByArrow() { }

    private IEnumerator Attacking(Enemy enemy)
    {
        State = AdventurerState.Attacking;
        GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(TIME_TO_ATTACK);
        enemy.Die();
        State = AdventurerState.Idle;
    }

    public void Attack(Enemy enemy)
    {
        navigation.Stop();
        StartCoroutine(Attacking(enemy));
    }

}
