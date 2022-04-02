using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DummyAdventurer : MonoBehaviour
{

    private List<CuriousInterest> curiousInterests;
    private List<GreedyInterest> greedyInterests;

    private LootTimer lootTimer;

    private void Start()
    {
        lootTimer = GetComponentInChildren<LootTimer>();

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
            if (greedyInterests[0].IsHazard())
            {
                Debug.Log("You dead good!");
            }
            else
            {
                StartCoroutine(LootProgress());
            }
        }
    }

    IEnumerator LootProgress()
    {
        int time = 0;
        lootTimer.ShowProgressBar();
        while (time < LootTimer.TIME_TO_LOOT)
        {
            time += 5;
            lootTimer.UpdateProgresBar(time);
            yield return new WaitForSeconds(0.05f);
        }
        greedyInterests[0].Loot();
        lootTimer.HideProgrssBar();
        yield return true;
    }


}
