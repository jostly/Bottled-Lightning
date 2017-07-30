using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryContainer : BatteryContainer
{
    public int rows;
    public int columns;

    public Vector2 spacing;
    public Vector2 offset;

    public float deliveryVelocity;
    public float returnVelocity;

    private Battery[,] storage;
    private int x;
    private int y;
    private float initialX;

    private void Start()
    {
        storage = new Battery[columns, rows];
        initialX = GetComponent<Rigidbody2D>().position.x;
        ResetContainer();
    }

    private void ResetContainer()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var t = storage[i, j];
                if (t != null)
                {
                    Destroy(t.gameObject);
                }
                storage[i, j] = null;
            }
        }
        x = columns - 1;
        y = 0;
    }

    public override void InsertBattery(Battery battery)
    {
        GameController.Instance.AddScore(battery.scoreValue);

        battery.GetComponent<Carryable>().Carry();
        battery.transform.parent = transform;
        battery.transform.localPosition = offset + new Vector2(x * spacing.x, y * spacing.y);
        battery.gameObject.tag = "Delivery";
        storage[x, y] = battery;

        y += 1;
        if (y >= rows)
        {
            y = 0;
            x -= 1;
        }

        if (x < 0)
        {
            Deliver();
        }
    }

    private void Deliver()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * deliveryVelocity;
        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Boundary")
        {
            GameController.Instance.RegisterDelivery(rows * columns);
            ResetContainer();
            GetComponent<Rigidbody2D>().velocity = Vector2.left * returnVelocity;
        }
    }

    private void FixedUpdate()
    {
        var pos = GetComponent<Rigidbody2D>().position;
        if (pos.x < initialX)
        {
            pos.x = initialX;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().position = pos;
        }
    }
}
