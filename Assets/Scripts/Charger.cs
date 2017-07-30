using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var battery = collision.GetComponent<Battery>();
        if (battery != null)
        {
            battery.Charge();
        }

    }


}
