using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public Battery batteryPrefab;
    public MinMax timeBetweenSpawns;
    public MinMax difficultyFactor;
    public float timeToMaxDifficulty;

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
        float startTime = Time.time;
        while (true)
        {
            while (spawnBlocked > 0)
            {
                yield return new WaitForFixedUpdate();
            }
            Instantiate(batteryPrefab, transform.position, Quaternion.identity);
            float diff = difficultyFactor.Lerp((Time.time - startTime) / timeToMaxDifficulty);
            yield return new WaitForSeconds(timeBetweenSpawns.Rnd() / diff);
        }
    }
}
