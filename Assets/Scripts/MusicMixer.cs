using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    public static MusicMixer instance;

    [Range(0f, 1f)]
    [SerializeField] float volume = 1;
    [SerializeField] AudioClip fullSong;
    [SerializeField] AudioClip startClip;
    [SerializeField] AudioClip lowClip;
    [SerializeField] AudioClip highClip;
    [SerializeField] AudioClip endClip;
    [SerializeField] float segmentLength = 2.4f;

    private AudioSource audioSource;
    private float switchTime = -1f;
    private string switchName;

    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        this.audioSource = GetComponent<AudioSource>();

        PlayFullSong();
    }

    void Update()
    {
        this.audioSource.volume = this.volume;

        if (this.switchTime >= 0f && this.audioSource.time >= this.switchTime) {
            this.switchTime = -1f;
            this.audioSource.Stop();
            switch(this.switchName) {
                case "low":
                    this.audioSource.clip = lowClip;
                    this.audioSource.loop = true;
                    this.audioSource.Play();
                    break;
                case "high": 
                    this.audioSource.clip = highClip;
                    this.audioSource.loop = true;
                    this.audioSource.Play();
                    break;
                case "end":
                    this.audioSource.clip = endClip;
                    this.audioSource.loop = false;
                    this.audioSource.Play();
                    break;
            }
        }
    }

    private float CalculateSwitchTime() {
        float clipLength = this.audioSource.clip.length;
        float currentTime = this.audioSource.time;
        return this.segmentLength * Mathf.Ceil(currentTime / segmentLength);
    }

    public void PlayFullSong() {
        this.audioSource.clip = fullSong;
        this.audioSource.loop = true;
        this.audioSource.Play();
    }

    public void StartMusic() {
        this.switchTime = startClip.length;
        this.switchName = "low";
        this.audioSource.Stop();
        this.audioSource.clip = startClip;
        this.audioSource.volume = volume;
        this.audioSource.loop = false;
        this.audioSource.Play();
    }

    public void QueueLow() {
        this.switchTime = CalculateSwitchTime();
        this.switchName = "low";
    }
    
    public void QueueHigh() {
        this.switchTime = CalculateSwitchTime();
        this.switchName = "high";
    }

    public void QueueEnd() {
        this.switchTime = CalculateSwitchTime();
        this.switchName = "end";
    }

    public void StopMusic() {
        this.switchTime = -1f;
        this.audioSource.Stop();
    }
}
