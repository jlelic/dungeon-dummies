using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var adventurer = collision.gameObject.GetComponent<Adventurer>();
        if(adventurer != null)
        {
            adventurer.Burn();
        }
    }
}
