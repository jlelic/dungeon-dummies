using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    void Update()
    {
        if(LevelManager.Instance.IsPlaying)
        {
            var texts = GetComponentsInChildren<Text>();
            iTween.MoveBy(gameObject, Vector3.right * 40, 5);

            Destroy(gameObject, 1);
        }
    }
}
