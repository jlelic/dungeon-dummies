using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public TileCoord EscapeCoord { get; private set; }
    int AdventurersRemaining;
    bool SomeoneDied;
    public bool IsPlaying { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate singleton Level Manager found");
            Destroy(this);
        }
    }

    private void Start()
    {
        var adventurers = GameObject.FindGameObjectsWithTag("Adventurer");
        AdventurersRemaining = adventurers.Length;
    }

    public void RegisterEscape(TileCoord coord)
    {
        EscapeCoord = coord;
    }

    public void OnAdventurerEscaped()
    {
        AdventurersRemaining--;
        if(AdventurersRemaining == 0)
        {
            if(SomeoneDied)
            {
                UIManager.Instance.RestartLevel();
            } else
            {
                UIManager.Instance.ShowGoodJobScreen();
            }
        }
    }

    public void OnAdventurerDeath()
    {
        AdventurersRemaining--;
        SomeoneDied = true;
        if(AdventurersRemaining == 0)
        {
            UIManager.Instance.RestartLevel();
        }
    }

    public void StartLevel()
    {
        IsPlaying = true;
        var adventurers = FindObjectsOfType<Adventurer>();
        foreach(var a in adventurers)
        {
            a.Activate();
        }
    }

    public void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
