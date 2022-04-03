using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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

        SetShowControlButtons(false);

        UncoverOverlay();
    }


    public bool HighlightTile(Vector3 position)
    {
        GridHighlight.gameObject.SetActive(true);
        var coord = Grid.GetTileCoordFromWorld(position);
        var objectTile = Grid.GetTile(coord, TileLayer.OBJECT);
        bool validPlacement = !Grid.IsBlocking(coord) && objectTile.Equals(TileType.EMPTY);

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

    public void RestartLevel()
    {
        var lights = FindObjectsOfType<Light2D>();
        foreach(var light in lights)
        {
            if(light.lightType == Light2D.LightType.Global) {
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
        Utils.tweenColor(Overlay, Color.black, 0.8f, 0.2f, ignoreTimeScale: true, callback: LevelManager.RestartLevel);
    }

    void UncoverOverlay()
    {
        Overlay.gameObject.SetActive(true);
        Overlay.color = Color.black;
        Utils.tweenColor(Overlay, Color.clear, 1.5f, ignoreTimeScale: true);
    }

    public void ShowGoodJobScreen()
    {
        Debug.Log("Good Job");
    }
}
