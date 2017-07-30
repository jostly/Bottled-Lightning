using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Carryable : MonoBehaviour
{
    private Rigidbody2D rb;

    public void Carry()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
    }

    public void Drop()
    {
        rb.isKinematic = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

}
