using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Lightning : MonoBehaviour
{
    public MinMax delayBetweenStrikes;
    public Light lightningLight;
    public float strikeIntensity;
    public float strikeDuration;
    public GameObject strikePrefab;
    public Vector3 strikeOffset;

    private AudioSource audioSource;
    private List<Battery> batteries;

    private void Start()
    {
        batteries = new List<Battery>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DoLightning());
    }

    private IEnumerator DoLightning()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenStrikes.Rnd());
            audioSource.Play();
            var endIntensity = 0f;
            var startTime = Time.time;
            ChargeBatteries();
            var strikes = new List<GameObject>();
            foreach (var battery in batteries)
            {
                var pos = battery.transform.position + strikeOffset;
                var strike = Instantiate(strikePrefab, pos, Quaternion.identity);
                strikes.Add(strike);
            }
            do
            {
                var intensity = Mathf.Lerp(strikeIntensity, endIntensity, (Time.time - startTime) / strikeDuration);
                lightningLight.intensity = intensity;
                yield return new WaitForEndOfFrame();
            } while (Time.time <= startTime + strikeDuration);
            lightningLight.intensity = endIntensity;
            foreach (var strike in strikes)
            {
                Destroy(strike);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var battery = collision.GetComponent<Battery>();
        if (battery != null)
        {
            batteries.Add(battery);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var battery = collision.GetComponent<Battery>();
        if (battery != null)
        {
            batteries.Remove(battery);
        }
    }

    private void ChargeBatteries()
    {
        foreach (var battery in batteries)
        {
            battery.Charge();
        }
    }
}
