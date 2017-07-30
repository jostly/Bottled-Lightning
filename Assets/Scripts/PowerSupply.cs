using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : BatteryContainer
{
    public string connectedTag;
    public float powerUsage;
    public Battery initialBattery;
    public Transform batteryPosition;
    public Transform dropPosition;

    public SpriteRenderer powerMeter;
    public float maxMeterWidth;

    public Color fullColor;
    public Color quarterColor;
    public Color emptyColor;

    public float warningLevel;

    private Battery battery;
    private float power { get {
            if (battery == null) return 0f;
            else return battery.currentCharge;
        }
    }
    private float maxPower { get
        {
            if (battery == null) return 0f;
            else return battery.maximumCharge;
        }
    }
    private float powerRatio {  get { return maxPower > 0 ? power / maxPower : 0; } }
    private bool outOfPower;

    override public void InsertBattery(Battery newBattery)
    {
        if (battery != null)
        {
            battery.transform.position = dropPosition.position;
            battery.GetComponent<Carryable>().Drop();
            battery = null;
        }
        if (newBattery != null)
        {
            battery = newBattery;
            battery.transform.position = batteryPosition.position;
            battery.GetComponent<Carryable>().Carry();
        }
    }

    private void Start()
    {
        outOfPower = false;
        InsertBattery(initialBattery);
        UpdatePowerMeter();
    }

    private void Update()
    {
        float currRatio = powerRatio;
        battery.Drain(powerUsage * Time.deltaTime);
        float newRatio = powerRatio;
        if (currRatio > warningLevel && newRatio <= warningLevel)
        {
            GetComponent<AudioSource>().Play();
        }
        UpdatePowerMeter();
    }

    private void UpdatePowerMeter()
    {
        float ratio = powerRatio;
        float width = Mathf.Lerp(0, maxMeterWidth, ratio);
        var s = powerMeter.size;
        s.x = width;
        powerMeter.size = s;

        Color col;
        if (ratio > 0.25f)
        {
            col = Color.Lerp(quarterColor, fullColor, (ratio - 0.25f) / 0.75f);
        } else
        {
            col = Color.Lerp(emptyColor, quarterColor, ratio / 0.25f);
        }

        powerMeter.color = col;

        if (!outOfPower && power == 0f)
        {
            outOfPower = true;
            TurnOff(GameObject.FindGameObjectsWithTag(connectedTag));
        } 
        if (outOfPower && power > 0f)
        {
            outOfPower = false;
            TurnOnLights(GameObject.FindGameObjectsWithTag(connectedTag));
        }

    }

    private void TurnOff(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            obj.GetComponent<IPoweredDevice>().PowerOff();
        }
    }

    private void TurnOnLights(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            obj.GetComponent<IPoweredDevice>().PowerOn();
        }
    }

}
