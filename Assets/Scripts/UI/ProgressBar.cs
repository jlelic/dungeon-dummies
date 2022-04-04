using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas)), RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour
{
    Canvas canvas;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        slider.maxValue = 1;
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

    public void ShowProgressBar(int maxValue)
    {
        if (!canvas.enabled)
        {
            canvas.enabled = true;
            slider.maxValue = maxValue;
        }
    }

    public void HideProgressBar()
    {
        if (canvas.enabled)
        {
            canvas.enabled = false;
            slider.maxValue = 1;
        }
    }
}