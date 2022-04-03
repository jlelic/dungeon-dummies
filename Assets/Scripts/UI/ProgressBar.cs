using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas)), RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour
{
    public static int TIME_TO_LOOT = 100;

    Canvas canvas;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        slider.maxValue = TIME_TO_LOOT;
        slider.value = 0;
    }

    public void UpdateProgresBar(int currentProgress)
    {
        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }
        slider.value = currentProgress;
    }

    public void ShowProgressBar()
    {
        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }
    }

    public void HideProgressBar()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
        }
    }
}