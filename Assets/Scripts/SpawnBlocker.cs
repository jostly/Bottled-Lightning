using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnBlocker : MonoBehaviour
{
    public BatterySpawner spawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawner.IncreaseBlockCount();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spawner.DecreaseBlockCount();
    }
}
