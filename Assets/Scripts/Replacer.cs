using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replacer : MonoBehaviour
{
    public string inputTag;
    public Replaceable output;
    public Rotator[] gears;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var obj = collision.gameObject;
        if (obj.tag == inputTag)
        {
            var replaced = Instantiate(output, obj.transform.position, obj.transform.rotation) as Replaceable;

            replaced.Replace(obj);

            obj.tag = "Destroyed";
            var battery = obj.GetComponent<Battery>();
            if (battery != null)
            {
                GameController.Instance.AddScore(battery.scoreValue);
            }
            
            Destroy(obj);

            foreach (var gear in gears)
            {
                gear.Trigger();
            }
        }
    }
}
