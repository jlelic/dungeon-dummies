using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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
