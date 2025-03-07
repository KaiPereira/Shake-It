using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBubble : MonoBehaviour
{
	private Customer customer;
	private OrderManager orderManager;

	// Start is called before the first frame update
	void Start()
	{
		orderManager = FindObjectOfType<OrderManager>();
		customer = GetComponentInParent<Customer>();
	}

	private void OnMouseDown()
	{
		if (customer.isReadyToSit)
		{
			orderManager.AddOrder(customer.order);
			customer.MoveToSeat();
		}
	}
}
