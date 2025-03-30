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

    public GameObject speechBubble;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private OrderManager orderManager;
    private ScoreManager scoreManager;

    public float orderTime;
    // This is the base and +1.5 is added for the randomised rate
    public float employeeRevenue = 5f;
    private float checkTime = 1f;
    private bool workingOnOrder = false;

    public void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        orderManager = FindObjectOfType<OrderManager>();
        scoreManager = FindObjectOfType<ScoreManager>();

        StartCoroutine(StartWorking());
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
        // I just run this every checkTime for performance reasons
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
        Order orderForEmployee = orderManager.WorkOnSecondOrder();

        if (orderForEmployee != null)
        {
            //Debug.Log("WORKING ON CUSTOMER: " + orderForEmployee.id);
            workingOnOrder = true;
            speechBubble.SetActive(true);

            MoveTo(cookingSpot1);

            yield return new WaitForSeconds(orderTime / 2);

            MoveTo(cookingSpot2);

            yield return new WaitForSeconds(orderTime / 2);
            
            // Finish the order for the customer
            CompleteOrder(orderForEmployee.id, orderForEmployee);

            workingOnOrder = false;
            speechBubble.SetActive(false);
        }
    }

    void CompleteOrder(string id, Order order)
    {
        // Give the payment
        float randomRevenue = Random.Range(employeeRevenue, employeeRevenue + 1.5f);
        randomRevenue = Mathf.Round(randomRevenue * 100f) / 100f;

        scoreManager.UpdateScore(randomRevenue * order.multiplier);

        // Give the customer their food
        GameObject customer = GameObject.Find(id);
        Customer customerScript = customer.GetComponent<Customer>();

        StartCoroutine(customerScript.GetFood());
    }
}
