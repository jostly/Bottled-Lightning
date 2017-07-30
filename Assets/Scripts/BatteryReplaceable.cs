using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Battery))]
public class BatteryReplaceable : Replaceable
{
    public override void Replace(GameObject source)
    {
        var sourceBattery = source.GetComponent<Battery>();
        var destinationBattery = GetComponent<Battery>();
        if (sourceBattery != null)
        {
            destinationBattery.maximumCharge = sourceBattery.maximumCharge;
            destinationBattery.currentCharge = sourceBattery.currentCharge;

            var sourceBody = source.GetComponent<Rigidbody2D>();
            var destinationBody = GetComponent<Rigidbody2D>();

            destinationBody.position = sourceBody.position;
            destinationBody.velocity = sourceBody.velocity;
            destinationBody.rotation = sourceBody.rotation;
            destinationBody.angularVelocity = sourceBody.angularVelocity;
        }
    }

}
