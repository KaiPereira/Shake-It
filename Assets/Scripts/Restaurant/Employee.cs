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

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private OrderManager orderManager;

    public float orderTime = 15f;
    private float checkTime = 1f;
    private bool workingOnOrder = false;

    public void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orderManager = GetComponent<OrderManager>();
    }

    public void MoveTo(Vector3 target, System.Action onMoveComplete = null)
	{
		targetPosition = target;
		StartCoroutine(MoveCoroutine(onMoveComplete));
	}

	private IEnumerator MoveCoroutine(System.Action onMoveComplete)
	{
		isMoving = true;
        animator.SetBool("IsMoving", isMoving);


		while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
		{
            Vector3 direction = (targetPosition - transform.position).normalized;

            animator.SetFloat("MoveX", direction.x);

			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
		
		transform.position = targetPosition;

        isMoving = false;
        animator.SetBool("IsMoving", isMoving);
        onMoveComplete?.Invoke();
    }

    IEnumerator StartWorking()
    {
        for (;;)
        {
            if (!workingOnOrder)
            {
                StartCoroutine(WorkOnOrder());
            }

            yield return new WaitForSeconds(checkTime);
        }
    }

    IEnumerator WorkOnOrder()
    {
        Order orderForEmployee = orderManager.GetSecondOrder();

        if (orderForEmployee != null)
        {
            workingOnOrder = true;

            MoveTo(cookingSpot1);

            yield return new WaitForSeconds(orderTime / 2);

            MoveTo(cookingSpot2);

            yield return new WaitForSeconds(orderTime / 2);

            workingOnOrder = false;
        }
    }
}
