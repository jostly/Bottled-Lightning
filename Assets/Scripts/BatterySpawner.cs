using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public Battery batteryPrefab;
    public float minTimeBetweenSpawns;
    public float maxTimeBetweenSpawns;

    private int spawnBlocked;

    private void Start()
    {
        spawnBlocked = 0;
        StartCoroutine(Spawn());        
    }

    public void IncreaseBlockCount()
    {
        spawnBlocked += 1;
    }

    public void DecreaseBlockCount()
    {
        spawnBlocked -= 1;
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            while (spawnBlocked > 0)
            {
                yield return new WaitForFixedUpdate();
            }
            Instantiate(batteryPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
        }
    }
}
