using System.Collections;
using UnityEngine;

public enum AdventurerState
{
    Idle,
    Moving,
    Interacting,
    Attacking,
    Thinking
}

public class Adventurer : MonoBehaviour
{
    private bool active = false;
    [SerializeField] GameObject BurnParticleEffectPrefab;
    new SpriteRenderer renderer;
    protected Navigation navigation;

    [SerializeField] ProgressBar progressBar;
    protected AdventurerState State;
    Interest CurrentInterest;

    protected virtual void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        navigation = GetComponent<Navigation>();
        State = AdventurerState.Idle;
    }

    public void Activate()
    {
        active = true;
    }

    protected virtual Interest GetNextInterest()
    {
        return LevelManager.Instance.Escape;
    }

    protected virtual void OnInterestedInteracted(Interest interest)
    {

    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        switch (State)
        {
            case AdventurerState.Idle:
                CurrentInterest = GetNextInterest();
                if (CurrentInterest == null)
                {
                    StartCoroutine(Thinking(0.5f));
                }
                else
                {
                    if (CurrentInterest.CanInteractFrom(transform.position))
                    {
                        StartCoroutine(Interacting(CurrentInterest));
                    }
                    else
                    {
                        navigation.Navigate(CurrentInterest);
                        State = AdventurerState.Moving;
                    }
                }
                break;
            case AdventurerState.Moving:
                if (!navigation.IsWalking && !(CurrentInterest is Escape))
                {
                    if (CurrentInterest.CanInteractFrom(transform.position))
                    {
                        StartCoroutine(Interacting(CurrentInterest));
                    }
                    else
                    {
                        StartCoroutine(Thinking(0.5f));
                    }
                }
                break;
        }
    }


    public void Burn()
    {
        active = false;
        navigation.Stop();
        Utils.tweenColor(renderer, Color.black, 0.2f);
        Instantiate(BurnParticleEffectPrefab, transform.position, Quaternion.identity, transform);
        iTween.ValueTo(gameObject, iTween.Hash(
            //"delay", 0.3f,
            "time", 1.5f,
            "from", 0f,
            "to", 1f,
            "onupdate", (System.Action<float>)((value) => { renderer.material.SetFloat("_Step", value); }),
            "oncomplete", (System.Action)(() => { OnDeath(); Destroy(gameObject); })
            ));
    }

    public void Smash()
    {
        active = false;
        navigation.Stop();
        // TODO - change animation to smash effect
        Utils.tweenColor(renderer, Color.black, 0.2f);
        Instantiate(BurnParticleEffectPrefab, transform.position, Quaternion.identity, transform);
        iTween.ValueTo(gameObject, iTween.Hash(
            //"delay", 0.3f,
            "time", 1.5f,
            "from", 0f,
            "to", 1f,
            "onupdate", (System.Action<float>)((value) => { renderer.material.SetFloat("_Step", value); }),
            "oncomplete", (System.Action)(() => { OnDeath(); Destroy(gameObject); })
            ));
    }

    public virtual void PierceByArrow()
    {
        active = false;
        navigation.Stop();
        OnDeath();
        Destroy(gameObject);
    }

    void OnDeath()
    {
        LevelManager.Instance.OnAdventurerDeath();
    }

    IEnumerator Thinking(float time)
    {
        yield return new WaitForSeconds(time);
        State = AdventurerState.Idle;
    }

    IEnumerator Interacting(Interest interest)
    {
        State = AdventurerState.Interacting;
        int time = 0;
        progressBar.ShowProgressBar();
        while (time < ProgressBar.TIME_TO_LOOT)
        {
            time += 5;
            progressBar.UpdateProgresBar(time);
            yield return new WaitForSeconds(0.05f);
        }
        interest.Interact(this);
        OnInterestedInteracted(interest);
        progressBar.HideProgressBar();
        State = AdventurerState.Idle;
        yield return true;
    }

}
