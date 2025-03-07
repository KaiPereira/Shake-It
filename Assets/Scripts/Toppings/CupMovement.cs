using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupMovement : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float moveX = Input.GetAxisRaw("Horizontal") * speed;
        rb.velocity = new Vector2(moveX, rb.velocity.y);
    }
}
