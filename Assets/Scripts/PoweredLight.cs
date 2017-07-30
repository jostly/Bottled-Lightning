using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PoweredLight : MonoBehaviour, IPoweredDevice
{
    public float fadeDuration = 1f;
    private float nominalIntensity;

    private void Start()
    {
        nominalIntensity = GetComponent<Light>().intensity;
    }

    public void PowerOff()
    {
        StartCoroutine(FadeIntensity(nominalIntensity, 0));
    }

    public void PowerOn()
    {
        StartCoroutine(FadeIntensity(0, nominalIntensity));
    }

    private IEnumerator FadeIntensity(float from, float to)
    {
        var light = GetComponent<Light>();
        var startTime = Time.time;

        yield return new WaitForEndOfFrame();
        while (true)
        {
            var t = (Time.time - startTime) / fadeDuration;
            light.intensity = Mathf.Lerp(from, to, t);
            if (t >= 1f)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
