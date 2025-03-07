using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private Collider2D rotatingObject;
    private Collider2D cup;
    private Rigidbody2D rb;

    void Start()
    {
        rotatingObject = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        cup = GameObject.Find("Cup").GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col != null && col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (cup != null && !rotatingObject.IsTouching(cup)) {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
