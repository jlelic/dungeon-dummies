using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] Color HoverColor;
    [SerializeField] Color HighlightColor;
    [SerializeField] int Level;
    Color originalColor;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        originalColor = text.color;
        if(Level == 0)
        {
            Level = PlayerPrefs.GetInt("maxLevel", 1);
            if(Level > 1)
            {
                text.text = "Continue";
            }
        } else
        {
            text.text = "Level " + Level;
        }
    }

    public void OnMouseEnter()
    {
        text.color = HoverColor;
    }

    public void OnMouseExit()
    {
        text.color = originalColor;
    }

    public void OnMouseClick()
    {
        StartCoroutine(BlinkAndSelectLevel());
    }

    public void SetLevelNumber(int level)
    {
        Level = level;
        if(text != null)
        {
            text.text = "Level " + level;
        }
    }

    public void SetCurrent()
    {
        //originalColor = HighlightColor;
        //text = GetComponent<Text>();
        //text.color = HighlightColor;
    }

    IEnumerator BlinkAndSelectLevel()
    {
        bool visible = true;
        MenuManager.Instance.LoadLevel(Level);
        for (int i = 0; i < 10; i++)
        {
            text.color = visible ? Color.clear : HoverColor;
            visible = !visible;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

}
