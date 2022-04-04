using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(AudioSource))]
public class BrusherBehavior : Adventurer
{
    private const float TIME_TO_ATTACK = 1.25f;
    private const float TIME_TO_BLOCK = 0.75f;


    protected override void Start()
    {
        base.Start();
    }

    public override void PierceByArrow()
    {
        navigation.Stop();
        StartCoroutine(Blocking());

    }
    public override void Kill()
    {
        navigation.Stop();
        StartCoroutine(Blocking());
    }


    private IEnumerator Attacking(Enemy enemy)
    {
        State = AdventurerState.Attacking;
        GetComponent<Animator>().SetTrigger("Attack");
        yield return new WaitForSeconds(TIME_TO_ATTACK);
        enemy.Die();
        State = AdventurerState.Idle;
    }

    private IEnumerator Blocking()
    {
        State = AdventurerState.Blocking;
        GetComponent<Animator>().SetTrigger("Block");
        yield return new WaitForSeconds(TIME_TO_BLOCK);
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
