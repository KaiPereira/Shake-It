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

	public Tilemap tilemap;
	public TileBase blenderTile;
	public TileBase potTile;

	public GameObject player;
	private Vector3 blenderPos;
	private Vector3 potPos;

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

		blenderPos = GetCookingTable(blenderTile);
		potPos = GetCookingTable(potTile);


		arrowManager.SpawnArrow(Vector3.zero, Vector3.zero);
	}

	public void Update()
	{
		Order nextOrder = GetNextOrder();

		if (nextOrder != null) {
			if (nextOrder.step == 0) {
				arrowManager.UpdateArrow(player.transform.position, blenderPos);
			} else if (nextOrder.step == 1) {
				arrowManager.UpdateArrow(player.transform.position, potPos);
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
			Debug.Log($"Completed order: {completedOrder.drinkName} with {completedOrder.toppings}");
		}
	}

	public void FailOrder()
	{
		if (orderQueue.Count > 0)
		{
			Order completedOrder = orderQueue.Dequeue();
			Debug.Log("FAILED ORDED");
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

	private Vector3 GetCookingTable(TileBase destoTile)
	{
		BoundsInt bounds = tilemap.cellBounds;
		TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

		for (int x = bounds.xMin; x < bounds.xMax; x++) {
			for (int y = bounds.yMin; y < bounds.yMax; y++) {
				Vector3Int tilePosition = new Vector3Int(x, y, 0);

				TileBase tile = tilemap.GetTile(tilePosition);

				if (tile == destoTile)
				{
					return tilemap.CellToWorld(tilePosition) + (tilemap.cellSize / 2f);
				}
			}
		}

		return Vector3.zero;
	}
}