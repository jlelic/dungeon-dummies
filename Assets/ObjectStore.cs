using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStore : MonoBehaviour
{
    [Header("Particle Effects")]
    [SerializeField] public GameObject BloodSquishParticleEffect;
    [SerializeField] public GameObject BloodSprayUpParticleEffect;
    [SerializeField] public GameObject BurnParticleEffect;
    [SerializeField] public GameObject BoneExplosionParticleEffect;
    [Header("Materials")]
    [SerializeField] public Material MaterialKilledSquish;
    [SerializeField] public Material MaterialKilledBurn;
    [Header("Sounds")]
    [SerializeField] public AudioClip SoundSquish;
    [SerializeField] public AudioClip SoundBurn;

    public static ObjectStore Instance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate singleton Object Store found");
            Destroy(this);
        }
    }
}
