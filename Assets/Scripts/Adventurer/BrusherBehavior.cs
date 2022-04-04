using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(AudioSource))]
public class BrusherBehavior : Adventurer
{
    private const float TIME_TO_ATTACK = 1.25f;

    protected override void Start()
    {
        base.Start();
    }

    public override void PierceByArrow() { }
    public override void Kill() { }


    private IEnumerator Attacking(Enemy enemy)
    {
        State = AdventurerState.Attacking;
        GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(TIME_TO_ATTACK);
        enemy.Die();
        State = AdventurerState.Idle;
    }

    public void PlayAttackSFX()
    {
        GetComponent<AudioSource>().Play();
    }

    public void Attack(Enemy enemy)
    {
        navigation.Stop();
        StartCoroutine(Attacking(enemy));
    }

}
