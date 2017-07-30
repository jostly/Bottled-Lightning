using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSupply : BatteryContainer
{
    public string connectedTag;
    public float powerUsage;
    public SpriteRenderer powerMeter;
    public Battery initialBattery;
    public Station pickupStation;
    public Station dropoffStation;
    public Vector3 batteryOffset;

    private Battery battery;
    private float power { get {
            if (battery == null) return 0f;
            else return battery.currentCharge;
        }
    }
    private bool outOfPower;

    override public void InsertBattery(Battery newBattery)
    {
        if (battery != null)
        {
            battery.transform.position = dropoffStation.transform.position;
            battery.GetComponent<Carryable>().Drop();
            battery = null;
        }
        if (newBattery != null)
        {
            battery = newBattery;
            battery.transform.position = transform.position + batteryOffset;
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
        battery.Drain(powerUsage * Time.deltaTime);
        UpdatePowerMeter();
    }

    private void UpdatePowerMeter()
    {
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
