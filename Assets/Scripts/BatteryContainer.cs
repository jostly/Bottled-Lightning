using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BatteryContainer : MonoBehaviour
{
    public string acceptTag;
    public float rejectMultiplier;

    public abstract void InsertBattery(Battery battery);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var battery = collision.GetComponent<Battery>();
        if (battery != null)
        {
            if (battery.tag == acceptTag)
            {
                InsertBattery(battery);
            }
            else
            {
                var rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    var vel = rb.velocity;
                    vel.x = -vel.x * rejectMultiplier;
                    rb.velocity = vel;
                }
            }
        }
    }


}
