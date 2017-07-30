using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Carryable))]
public class Battery : MonoBehaviour
{
    public float maximumCharge;
    public float currentCharge;
    public SpriteRenderer indicator;
    public Color chargedTint;
    public Color depletedTint;
    public int scoreValue;

    public void Charge()
    {
        currentCharge = maximumCharge;
        UpdateIndicator();
    }

    public void Drain(float amount)
    {
        currentCharge = Mathf.Max(currentCharge - amount, 0f);
        UpdateIndicator();
    }

	void Start ()
    {
        UpdateIndicator();
	}
	
    private void UpdateIndicator()
    {
        var t = currentCharge / maximumCharge;
        indicator.color = Color.Lerp(depletedTint, chargedTint, Mathf.Pow(t, 0.7f));
    }
}
