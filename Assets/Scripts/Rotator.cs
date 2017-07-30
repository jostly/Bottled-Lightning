using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float angularSpeed;
    public float triggeredDuration;

    public void Trigger()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        var startTime = Time.time;
        var startAngle = transform.rotation.eulerAngles.z;
        var endAngle = startAngle + angularSpeed * triggeredDuration;
        while (Time.time <= startTime + triggeredDuration)
        {
            var a = Mathf.Lerp(startAngle, endAngle, (Time.time - startTime) / triggeredDuration);
            transform.rotation = Quaternion.Euler(0, 0, a);
            yield return new WaitForEndOfFrame();
        }
    }
}
