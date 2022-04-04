using System;
using System.Collections;
using UnityEngine;

public enum AdventurerState
{
    Idle,
    Moving,
    Leaving,
    Interacting,
    Attacking,
    Blocking,
    Thinking
}

public class Adventurer : MonoBehaviour
{
    const float AI_UPDATE_TIME = 0.2f;

    private bool active = false;
    [SerializeField] AudioClip FallSound;
    protected new SpriteRenderer renderer;
    protected Navigation navigation;
    protected Animator animator;
    protected AudioSource audioSource;
    protected ObjectStore store;
    protected GridManager grid;

    [SerializeField] ProgressBar progressBar;
    [SerializeField] protected AdventurerState State;
    Interest CurrentInterest;

    private float sinceLastAiUpdate;


    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        navigation = GetComponent<Navigation>();
        audioSource = GetComponent<AudioSource>();
        store = ObjectStore.Instance;
        grid = GridManager.Instance;

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

    protected virtual void AIUpdate()
    {

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
                        if (CurrentInterest is Escape)
                        {
                            navigation.ForceNavigate(CurrentInterest);
                            State = AdventurerState.Leaving;
                        }
                        else
                        {
                            navigation.Navigate(CurrentInterest);
                            State = AdventurerState.Moving;
                        }
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
            case AdventurerState.Leaving:
                if (!navigation.IsWalking)
                {
                    State = AdventurerState.Idle;
                }
                break;
        }
        sinceLastAiUpdate += Time.deltaTime;
        if (sinceLastAiUpdate > AI_UPDATE_TIME)
        {
            sinceLastAiUpdate = 0;
            AIUpdate();
        }

    }

    protected bool CanSee(Interest interest)
    {
        return CanSee(interest.Coord);
    }

    protected bool CanSee(TileCoord coord)
    {
        return grid.IsLos(transform.position, coord);
    }

    public void Burn()
    {
        active = false;
        navigation.Stop();
        renderer.material = store.MaterialKilledBurn;
        Utils.tweenColor(renderer, Color.black, 0.2f);
        Utils.PlayAudio(audioSource, store.SoundBurn, true);
        Instantiate(store.BurnParticleEffect, transform.position, Quaternion.identity, transform);
        iTween.ValueTo(gameObject, iTween.Hash(
            //"delay", 0.3f,
            "time", 1.5f,
            "from", 0f,
            "to", 1f,
            "onupdate", (System.Action<float>)((value) => { renderer.material.SetFloat("_Step", value); }),
            "oncomplete", (System.Action)(() => { OnDeath(); Destroy(gameObject); })
            ));
    }

    public void Fall(Vector3 holePosition, bool spikes)
    {
        active = false;
        navigation.Stop();
        Utils.PlayAudio(audioSource, FallSound, true);
        if (spikes)
        {
            Utils.tweenColor(renderer, new Color(1, 0.2f, 0.2f), 0.8f);
        }
        float targetScale = spikes ? 0.5f : 0f;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", holePosition,
            "time", 0.4f,
            "easetype", iTween.EaseType.easeOutQuad
            ));
        iTween.RotateAdd(gameObject, new Vector3(0, 0, UnityEngine.Random.Range(-60f, -85f)), 0.4f);
        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", Vector3.one * targetScale,
            "time", 0.4f,
            "easetype", iTween.EaseType.easeOutQuad,
            "oncomplete", (Action)(() =>
            {
                if (spikes)
                {
                    Utils.PlayAudio(audioSource, store.SoundSquish, true);
                    Instantiate(ObjectStore.Instance.BloodSprayUpParticleEffect, transform.position, Quaternion.identity, transform);
                }
                OnDeath();
            })
            ));
    }

    public void Squish()
    {
        active = false;
        navigation.Stop();
        renderer.material = store.MaterialKilledSquish;
        // TODO - change animation to smash effect
        Utils.PlayAudio(audioSource, store.SoundSquish, true);
        Utils.tweenColor(renderer, new Color(1, 0.2f, 0.2f), 0.8f);
        renderer.sortingOrder = 2;
        Instantiate(ObjectStore.Instance.BloodSquishParticleEffect, transform.position, Quaternion.identity, transform);
        iTween.ValueTo(gameObject, iTween.Hash(
            //"delay", 0.3f,
            "time", 0.25f,
            "from", 0f,
            "to", 1f,
            "easetype", iTween.EaseType.linear,
            "onupdate", (System.Action<float>)((value) => { renderer.material.SetFloat("_Step", value); }),
            "oncomplete", (System.Action)(() => { OnDeath(); })
            ));
    }

    public virtual void PierceByArrow()
    {
        Kill();
    }

    public virtual void Kill()
    {
        active = false;
        navigation.Stop();
        Utils.PlayAudio(audioSource, store.SoundSquish, true);
        Utils.tweenColor(renderer, new Color(1, 0.5f, 0.5f), 1.2f);
        Instantiate(ObjectStore.Instance.BloodSprayUpParticleEffect, transform.position, Quaternion.identity, transform);
        iTween.RotateAdd(gameObject, new Vector3(0, 0, UnityEngine.Random.Range(80, 100f)), 0.3f);
        iTween.ScaleTo(gameObject, iTween.Hash(
            "scale", new Vector3(0.7f, 1, 1),
            "time", 0.4f,
            "easetype", iTween.EaseType.easeOutQuad,
            "oncomplete", (Action)(() =>
            {
                OnDeath();
            })
            ));
    }

    void OnDeath()
    {
        GetComponent<Collider2D>().enabled = false;
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
        animator.SetBool("Interacting", true);
        int time = 0;
        progressBar.ShowProgressBar(interest.TimeToInteract);
        interest.BeginInteract();
        while (time < interest.TimeToInteract)
        {
            time += 5;
            progressBar.UpdateProgresBar(time);
            yield return new WaitForSeconds(0.05f);
        }
        interest.Interact(this);
        OnInterestedInteracted(interest);
        progressBar.HideProgressBar();
        animator.SetBool("Interacting", false);
        State = AdventurerState.Idle;
        yield return true;
    }

}
