using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public const int MAX_LEVELS = 10;

    public Escape Escape { get; private set; }
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
        Time.timeScale = 1;
        var adventurers = GameObject.FindGameObjectsWithTag("Adventurer");
        AdventurersRemaining = adventurers.Length;
    }

    public void RegisterEscape(Escape escape)
    {
        Escape = escape;
    }

    public void OnAdventurerEscaped()
    {
        AdventurersRemaining--;
        if (AdventurersRemaining == 0)
        {
            if (SomeoneDied)
            {
                UIManager.Instance.RestartLevel();
            }
            else
            {
                int levelId = SceneManager.GetActiveScene().buildIndex;
                Time.timeScale = 1;
                PlayerPrefs.SetInt("maxLevel", levelId + 1);
                UIManager.Instance.ShowGoodJobScreen(levelId);
            }
            MusicMixer.instance.QueueLow();
        }
    }

    public void OnAdventurerDeath()
    {
        AdventurersRemaining--;
        SomeoneDied = true;
        if (AdventurersRemaining == 0)
        {
            UIManager.Instance.RestartLevel(3f);
        }
    }

    public void StartLevel()
    {
        IsPlaying = true;
        var adventurers = FindObjectsOfType<Adventurer>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var a in adventurers)
        {
            a.Activate();
        }
        foreach (Enemy e in enemies)
        {
            e.Activate();
        }
        MusicMixer.instance.QueueHigh();
    }

    public void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void LoadLevel(int levelNum)
    {
        SceneManager.LoadScene("Scenes/Levels/" + levelNum);
    }
}
