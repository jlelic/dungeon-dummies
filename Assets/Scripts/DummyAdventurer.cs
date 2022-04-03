using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DummyAdventurer : MonoBehaviour
{

    private List<CuriousInterest> curiousInterests;
    private List<GreedyInterest> greedyInterests;

    private ProgressBar lootTimer;

    private void Start()
    {
        lootTimer = GetComponentInChildren<ProgressBar>();

        curiousInterests = new List<CuriousInterest>();
        greedyInterests = new List<GreedyInterest>();
        foreach (CuriousInterest item in FindObjectsOfType<CuriousInterest>())
        {
            curiousInterests.Add(item);
        }
        foreach (GreedyInterest item in FindObjectsOfType<GreedyInterest>())
        {
            greedyInterests.Add(item);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var interest in curiousInterests)
            {
                interest.Interact();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(LootProgress());
        }
    }

    IEnumerator LootProgress()
    {
        int time = 0;
        lootTimer.ShowProgressBar();
        while (time < ProgressBar.TIME_TO_LOOT)
        {
            time += 5;
            lootTimer.UpdateProgresBar(time);
            yield return new WaitForSeconds(0.05f);
        }
        greedyInterests[0].Interact(null);
        lootTimer.HideProgressBar();
        yield return true;
    }


}
