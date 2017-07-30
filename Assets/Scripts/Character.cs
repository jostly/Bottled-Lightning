using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Station station;
    public float movementSpeed;

    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    public Vector3 offset;
    public Vector3 carryOffset;

    public float throwVelocity;

    public float maxCatchVelocity;

    private bool moving;
    private bool waitingForPickup;
    private Carryable carrying;


	void Start ()
    {
        carrying = null;
        transform.position = station.transform.position + offset;
        moving = false;
        waitingForPickup = false;
	}
	
	void Update ()
    {
		if (!moving)
        {
            if (Input.GetKey(up))
            {
                station.TakeAction(this, Action.Up);
            } else if (Input.GetKey(down))
            {
                station.TakeAction(this, Action.Down);
            } else if (Input.GetKey(left))
            {
                station.TakeAction(this, Action.Left);
            } else if (Input.GetKey(right))
            {
                station.TakeAction(this, Action.Right);
            }
        }
        
        if (carrying != null)
        {
            carrying.transform.position = transform.position + carryOffset;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var c = collision.gameObject.GetComponent<Carryable>();
        var rb = c.GetComponent<Rigidbody2D>();
        if (c != null && rb != null && carrying == null && Mathf.Abs(rb.velocity.y) <= maxCatchVelocity)
        {
            waitingForPickup = false;
            carrying = c;
            c.Carry();
            GetComponent<AudioSource>().Play();
        }
    }

    public void DropOff(Action direction)
    {
        if (carrying == null)
        {
            return;
        }
        float lean = 0f;
        Vector2 throwDirection = Vector2.zero;
        if (direction == Action.Left)
        {
            lean = 30f;
            throwDirection = new Vector2(-0.7f, 0.7f);
        }
        else if (direction == Action.Right)
        {
            lean = -30f;
            throwDirection = new Vector2(0.7f, 0.7f);
        }
        if (direction == Action.Left) lean = 30f;
        else if (direction == Action.Right) lean = -30f;
        StartCoroutine(DroppingOff(lean, throwDirection));
    }

    public void WaitForPickup(Action direction)
    {
        /* TODO: Should eliminate this action
        if (carrying)
        {
            return;
        }
        float lean = 0f;
        if (direction == Action.Left) lean = 30f;
        else if (direction == Action.Right) lean = -30f;
        StartCoroutine(WaitingForPickup(lean));
        */
    }

    public void MoveToStation(Station station)
    {
        StartCoroutine(MovingTo(station));
    }

    private IEnumerator DroppingOff(float lean, Vector2 throwDirection)
    {
        moving = true;
        transform.rotation = Quaternion.Euler(0, 0, lean);
        if (carrying != null)
        {
            carrying.transform.position = transform.position + transform.rotation * Vector3.up;
        }
        yield return new WaitForFixedUpdate();
        if (carrying != null)
        {
            carrying.Drop();
            var rb = carrying.GetComponent<Rigidbody2D>();
            rb.velocity = throwDirection * throwVelocity;
            carrying = null;
        }
        yield return new WaitForSeconds(0.2f);
        transform.rotation = Quaternion.identity;
        moving = false;
    }

    private IEnumerator WaitingForPickup(float lean)
    {
        waitingForPickup = true;
        transform.rotation = Quaternion.Euler(0, 0, lean);
        while (waitingForPickup && !moving)
        {
            yield return new WaitForEndOfFrame();
        }
        waitingForPickup = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    private IEnumerator MovingTo(Station targetStation)
    {
        moving = true;
        Vector3 startPosition = transform.position;
        float distance = (targetStation.transform.position + offset - startPosition).magnitude;
        float startTime = Time.time;
        float duration = distance / movementSpeed;
        yield return new WaitForEndOfFrame();
        while (true)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startPosition, targetStation.transform.position + offset, t);
            if (t >= 1f)
            {
                moving = false;
                station = targetStation;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
