using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public bool active = false;
    [SerializeField] GameObject BurnParticleEffectPrefab;
    new SpriteRenderer renderer;
    Navigation navigation;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        navigation = GetComponent<Navigation>();
    }

    public void Activate()
    {
        active = true;
        navigation.Navigate(new TileCoord(12, 1));
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
            "oncomplete", (System.Action)(() => { Destroy(gameObject); })
            ));
    }
}
