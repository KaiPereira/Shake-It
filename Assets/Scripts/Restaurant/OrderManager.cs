using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OrderManager : MonoBehaviour
{
	public static OrderManager Instance;

	private Queue<Order> orderQueue = new Queue<Order>();
	public float orderTimeLimit = 30f;
	private ArrowManager arrowManager;
	private ScoreManager scoreManager;

	public Tilemap tilemap;

	public GameObject player;
	public Vector3 rhythmPos;
	public Vector3 toppingPos;
	
	public float base_score = 10f;

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void Start()
	{
		arrowManager = GetComponent<ArrowManager>();
		scoreManager = FindObjectOfType<ScoreManager>();

		arrowManager.SpawnArrow(Vector3.zero, Vector3.zero);
	}

	public void Update()
	{
		Order nextOrder = GetNextOrder();

		if (nextOrder != null) {
			if (nextOrder.step == 0) {
				arrowManager.UpdateArrow(player.transform.position, rhythmPos);
			} else if (nextOrder.step == 1) {
				arrowManager.UpdateArrow(player.transform.position, toppingPos);
			}
		} else {
			arrowManager.UpdateArrow(new Vector3(100, 100, 100), new Vector3(100, 100, 100));
		}
	}

	public void AddOrder(Order order)
	{
		orderQueue.Enqueue(order);

		Debug.Log($"New Order: {order.drinkName} with {order.toppings}");
	}

	public Order GetNextOrder()
	{
		if (orderQueue.Count > 0)
		{
			return orderQueue.Peek();
		}
		return null;
	}

	public void CompleteOrder()
	{
		if (orderQueue.Count > 0)
		{
			Order completedOrder = orderQueue.Dequeue();

			float avg_accuracy = completedOrder.accuracy / 2;
			float total_score = base_score * avg_accuracy * completedOrder.multiplier;

			scoreManager.UpdateScore(total_score);

			Customer customerScript = GameObject.Find(completedOrder.id).GetComponent<Customer>();

			StartCoroutine(customerScript.GetFood());

			Debug.Log($"Completed order: {completedOrder.drinkName} with {completedOrder.toppings}");
		}
	}

	public void FailOrder(string id)
	{
		if (orderQueue.Count > 0)
		{
			Queue<Order> newQueue = new Queue<Order>();

			foreach (Order order in orderQueue)
			{
				if (order.id != id)
				{
					newQueue.Enqueue(order);
				}
			}

			orderQueue = newQueue;
		}
	}

	public void UpdateOrderScore(float accuracy)
	{
		if (orderQueue.Count > 0)
		{
			orderQueue.Peek().accuracy += accuracy;

			Debug.Log("CURRENT ORDER RATING: " + orderQueue.Peek().accuracy);
		}
	}

	public void CompleteStep()
	{
		if (orderQueue.Count > 0) {
			orderQueue.Peek().step = 1;
		}
	}
	
	public int GetOrderCount()
	{
		return orderQueue.Count;
	}

	public void ClearOrders()
	{
		foreach (Order order in orderQueue)
		{
			GameObject customer = GameObject.Find(order.id);

			Destroy(customer);
		}

		orderQueue.Clear();
	}
}