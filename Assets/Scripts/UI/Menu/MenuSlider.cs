using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlider : MonoBehaviour
{
    [SerializeField] GameObject SlidingLevel;
    [SerializeField] float SlidingSpeed = 2f;
    float LevelWidth = 64;
    List<GameObject> SlidingLevels;

    private void Start()
    {
        SlidingLevels = new List<GameObject>();
        SlidingLevels.Add(SlidingLevel);
        var copy = Instantiate(SlidingLevel);
        copy.transform.SetParent(transform);
        copy.transform.position = SlidingLevel.transform.position + Vector3.right * LevelWidth;
        SlidingLevels.Add(copy);
    }

    private void Update()
    {
        foreach(var l in SlidingLevels)
        {
            l.transform.position += Vector3.left * SlidingSpeed*Time.deltaTime;
            if(l.transform.position.x < -LevelWidth)
            {
                var resetBy = Vector3.right * LevelWidth * 2;
                l.transform.position += resetBy;
            }
        }
    }
}
