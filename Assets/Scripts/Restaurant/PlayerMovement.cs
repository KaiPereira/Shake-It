using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public int speed = 10;
	private Rigidbody2D characterBody;
	private Vector2 velocity;
	private Vector2 inputMovement;
	private Animator animator;
	bool isMoving;

	void Start()
	{
		velocity = new Vector2(speed, speed);
		characterBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		inputMovement = new Vector2 (
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical")
		);

		isMoving = inputMovement != Vector2.zero;

		animator.SetFloat("MoveX", inputMovement.x);
		animator.SetFloat("MoveY", inputMovement.y);
		animator.SetBool("IsMoving", isMoving);
	}

	private void FixedUpdate()
	{
		Vector2 delta = inputMovement * velocity * Time.deltaTime;
		Vector2 newPosition = characterBody.position + delta;
		characterBody.MovePosition(newPosition);
	}
}
