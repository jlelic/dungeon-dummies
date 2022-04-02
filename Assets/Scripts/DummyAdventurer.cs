using UnityEngine;
using System.Collections.Generic;

public class DummyAdventurer : MonoBehaviour
{
    private List<AdventurerInterest> adventurerInterests;

    private void Start()
    {
        adventurerInterests = new List<AdventurerInterest>();
        foreach (AdventurerInterest item in FindObjectsOfType<AdventurerInterest>())
        {
            adventurerInterests.Add(item);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            foreach (var interest in adventurerInterests)
            {
                interest.Interact();
            }
        }
    }


}
