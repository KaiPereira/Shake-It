using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public Vector3 cookingSpot1;
    public Vector3 cookingSpot2;

    public float moveSpeed = 2f;
	private Vector3 targetPosition;
	private bool isMoving = false;

    public void MoveTo(Vector3 target, System.Action onMoveComplete = null)
	{
		targetPosition = target;
		StartCoroutine(MoveCoroutine(onMoveComplete));
	}

	private IEnumerator MoveCoroutine(System.Action onMoveComplete)
	{
		isMoving = true;

		while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
		
		transform.position = targetPosition;

        isMoving = false;
        onMoveComplete?.Invoke();
    }
}
