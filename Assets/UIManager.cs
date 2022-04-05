using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public UIManager Instance;

    [SerializeField] SpriteRenderer Overlay;
    [SerializeField] SpriteRenderer GridHighlight;
    [SerializeField] Sprite OkGridHighlightSprite;
    [SerializeField] Sprite NoGridHighlightSprite;
    [SerializeField] UIButton ButtonStart;
    [SerializeField] UIButton ButtonRestart;
    [SerializeField] UIButton ButtonPlay;
    [SerializeField] UIButton ButtonFast;
    [SerializeField] UIButton ButtonBack;
    [SerializeField] Text LevelEndText;
    [SerializeField] GameObject LevelEndAnimation;
    [SerializeField] GameObject Borders;


    GridManager Grid;
    LevelManager LevelManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate singleton UI Manager found");
            Destroy(this);
        }
        StopHighlight();
        Overlay.gameObject.SetActive(true);
        Overlay.color = Color.black;
        Borders.SetActive(true);
    }

    private void Start()
    {
        ButtonStart.gameObject.SetActive(true);
        SetShowControlButtons(true);

        LevelManager = LevelManager.Instance;
        Grid = GridManager.Instance;
        ButtonStart.AddOnClickListener(OnStartClicked);
        ButtonFast.AddOnClickListener(OnFastClicked);
        ButtonPlay.AddOnClickListener(OnPlayClicked);
        ButtonRestart.AddOnClickListener(OnRestartClicked);
        ButtonBack.AddOnClickListener(OnBackClicked);

        SetShowControlButtons(false);

        UncoverOverlay();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnBackClicked();
            return;
        }
    }

    public bool HighlightTile(Vector3 position)
    {
        GridHighlight.gameObject.SetActive(true);
        var coord = Grid.GetTileCoordFromWorld(position);
        var objectTile = Grid.GetTile(coord, TileLayer.OBJECT);
        bool validPlacement = !Grid.IsBlocking(coord) && objectTile.Equals(TileType.EMPTY) && !Grid.IsTileEnemy(coord);

        if (validPlacement)
        {
            GridHighlight.sprite = OkGridHighlightSprite;
        }
        else
        {
            GridHighlight.sprite = NoGridHighlightSprite;
        }
        var tileWorldPos = Grid.GetWorldPosFromTile(coord);
        GridHighlight.transform.position = tileWorldPos;
        return validPlacement;
    }

    public void StopHighlight()
    {
        GridHighlight.gameObject.SetActive(false);
    }

    private void SetShowControlButtons(bool show)
    {
        ButtonPlay.gameObject.SetActive(show);
        ButtonFast.gameObject.SetActive(show);
        ButtonRestart.gameObject.SetActive(show);
    }

    void OnStartClicked()
    {
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        ButtonStart.LightUp(0.5f);
        yield return new WaitForSecondsRealtime(0.5f);
        ButtonStart.gameObject.SetActive(false);
        SetShowControlButtons(true);
        yield return new WaitForSecondsRealtime(0.2f);
        ButtonPlay.Toggle();
        LevelManager.Instance.StartLevel();
    }

    void OnFastClicked()
    {
        Time.timeScale = 3;
        ButtonFast.Toggle();
        ButtonPlay.Untoggle();
    }

    void OnPlayClicked()
    {
        Time.timeScale = 1;
        ButtonFast.Untoggle();
        ButtonPlay.Toggle();
    }

    void OnRestartClicked()
    {
        RestartLevel();
    }

    void OnBackClicked()
    {
        InvokeAfterOverlay(-0.2f, () => {
            LevelManager.Instance.LoadLevel(0);
            MusicMixer.instance.PlayFullSong();
        });
    }

    void InvokeAfterOverlay(float delay, Action callback)
    {
        var lights = FindObjectsOfType<Light2D>();
        foreach (var light in lights)
        {
            if (light.lightType == Light2D.LightType.Global)
            {
                continue;
            }
            iTween.ValueTo(light.gameObject, iTween.Hash(
                "from", light.intensity,
                "to", 0,
                "time", 0.6f,
                "ignoretimescale", true,
                "onupdate", (System.Action<float>)((value) => { light.intensity = value; })
                ));
        }
        Overlay.gameObject.SetActive(true);
        Overlay.color = Color.clear;
        Utils.tweenColor(Overlay, Color.black, 0.8f, 0.2f + delay, ignoreTimeScale: true, callback: callback);
    }

    public void RestartLevel(float delay = 0)
    {
        InvokeAfterOverlay(delay, LevelManager.RestartLevel);
    }

    void UncoverOverlay()
    {
        Overlay.gameObject.SetActive(true);
        Overlay.color = Color.black;
        Utils.tweenColor(Overlay, Color.clear, 1.5f, ignoreTimeScale: true);
    }

    public void ShowGoodJobScreen(int numLevel)
    {
        InvokeAfterOverlay(0f, () =>
        {
            LevelEndAnimation.transform.parent.gameObject.SetActive(true);
            LevelEndAnimation.SetActive(true);
            LevelEndText.gameObject.SetActive(true);

            var animators = LevelEndAnimation.GetComponentsInChildren<Animator>();
            foreach (var a in animators)
            {
                a.SetBool("Walking", true);
            }

            var easeType = iTween.EaseType.easeOutCubic;
            var timeTransitions = 0.8f;
            var timeMain = 3.5f;
            var animPos = LevelEndAnimation.transform.position;
            var textPos = LevelEndText.transform.position;
            iTween.MoveTo(LevelEndAnimation, iTween.Hash(
                "position", new Vector3(0, animPos.y),
                "time", timeTransitions,
                "easetype", easeType,
                "ignoretimescale", true
                ));
            LevelEndText.text = "Room " + numLevel + " passed";
            iTween.MoveTo(LevelEndText.gameObject, iTween.Hash(
                "position", new Vector3(0, textPos.y),
                "time", timeTransitions,
                "easetype", easeType,
                "ignoretimescale", true
                ));

            iTween.MoveTo(LevelEndAnimation, iTween.Hash(
                "position", new Vector3(4, animPos.y),
                "time", timeMain,
                "easetype", iTween.EaseType.linear,
                "ignoretimescale", true,
                "delay", timeTransitions
                ));
            iTween.MoveTo(LevelEndText.gameObject, iTween.Hash(
                "position", new Vector3(-4, textPos.y),
                "time", timeMain,
                "easetype", iTween.EaseType.linear,
                "ignoretimescale", true,
                "delay", timeTransitions, // I HATE MY LIFE
                "oncomplete", (Action)(() =>
                {
                    iTween.MoveBy(LevelEndAnimation, iTween.Hash(
                        "amount", Vector3.right * 20,
                        "time", timeTransitions,
                        "easetype", easeType,
                        "ignoretimescale", true
                        ));
                    iTween.MoveBy(LevelEndText.gameObject, iTween.Hash(
                        "amount", Vector3.left * 20,
                        "time", timeTransitions,
                        "easetype", easeType,
                        "ignoretimescale", true,
                        "oncomplete", ((Action)(() => { LevelManager.LoadLevel(numLevel + 1); }))
                        ));
                })
                ));




            //                "oncomplete", ((Action)(() => { LevelManager.LoadLevel(numLevel + 1); }))
        });
    }
}
