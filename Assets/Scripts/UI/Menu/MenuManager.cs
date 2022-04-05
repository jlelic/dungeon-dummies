using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject LevelGridContainer;
    [SerializeField] GameObject LevelButtonPrefab;
    [SerializeField] SpriteRenderer Overlay;

    public static MenuManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Duplicate singleton Menu Manager found");
            Destroy(this);
        }
    }

    void Start()
    {
        Overlay.gameObject.SetActive(false);
        var animators = GameObject.FindObjectsOfType<Animator>();
        foreach(var a in animators)
        {
            a.SetBool("Walking", true);
        }

        int maxLevel = PlayerPrefs.GetInt("maxLevel", 1);
        for (int i = 1; i <= 10; i++)
        {
            var newButtonObject = Instantiate(LevelButtonPrefab);
            newButtonObject.transform.SetParent(LevelGridContainer.transform,false);
            //newButtonObject.transform.localScale = Vector3.one;
            //newButtonObject.transform.position = Vector3.zero;
            var button = newButtonObject.GetComponent<LevelButton>();
            button.SetLevelNumber(i);
            if (i == maxLevel)
            {
                button.SetCurrent();
            }
        }
    }


    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }


    public void LoadLevel(int levelNum)
    {
        Overlay.gameObject.SetActive(true);
        Overlay.color = Color.clear;
        Utils.tweenColor(Overlay,Color.black,1,0.6f,iTween.EaseType.linear, true,() => LevelManager.Instance.LoadLevel(levelNum));
    }
}
