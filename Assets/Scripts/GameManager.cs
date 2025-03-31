using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public GameObject timerPrefab;
	public ParticleSystem effect;
	public Material bronzeCoin;
	public Material silverCoin;
	public Material goldCoin;
	public Material trophyCoin;

	public Sprite customerSprite;
	public WaitingQueue waitingQueue;
	private int waitingQueueSize = 1;
	List<Vector3> waitingQueuePositionList = new List<Vector3>();
	float positionSize = 1f;

	public OrderManager orderManager;
	public SeatManager seatManager;

	public Sprite angryFace;
	public Sprite happyFace;

	public int currentStreak;
	public int totalClicked;
	public int totalSpawned;

	public int toppingScore;
	public int toppingScoreFiveStar = 12;

	public int customerLevelUpgrade = 0;
	public int customerRate = 0;
	public int restaurantLevel = 0;

	public AudioSource doorbell;
	private Vector3 spawnPosition = new Vector3(-24, -5, 0);

	private Tilemap furnishingTilemap;
	private Tilemap decorationsTilemap;
	public TileBase bobaTile;

	public GameObject rhythmPrefab;
	public GameObject toppingPrefab;

	public GameObject[] restaurants;
	public List<Customer> customers = new List<Customer>();
	
	private Vector3 queueStart;

	private Vector3 cookingSpot1;
	private Vector3 cookingSpot2;

	public GameObject employeePrefab;
	private List<Employee> employees = new List<Employee>();
	private float employeeOrderTime = 60f;
	private float employeeRevenue = 5f;

	private GameObject interaction1Ref;
	private GameObject interaction2Ref;

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

	private void Start()
	{
		furnishingTilemap = GameObject.Find("Furnishing").GetComponent<Tilemap>();
		decorationsTilemap = GameObject.Find("Decorations").GetComponent<Tilemap>();

		seatManager = GetComponent<SeatManager>();

		AddInteractionPrompt();
		GetWaitingQueueStart();

		UpdateWaitingQueue();

		StartCoroutine(SpawnCustomers());
	}

	private void UpdateWaitingQueue()
	{
		waitingQueuePositionList.Clear();
		GetWaitingQueueStart();

		for (int i = 0; i < waitingQueueSize; i++) {
			waitingQueuePositionList.Add(queueStart + new Vector3(0f, -i * positionSize, 0f));
		}

		waitingQueue.Initialize(waitingQueuePositionList);
	}

	private void GetWaitingQueueStart()
	{

		BoundsInt bounds = decorationsTilemap.cellBounds;
		TileBase[] allTiles = decorationsTilemap.GetTilesBlock(bounds);

		for (int x = bounds.xMin; x < bounds.xMax; x++)
		{
			for (int y = bounds.yMin; y < bounds.yMax; y++)
			{
				Vector3Int tilePosition = new Vector3Int(x, y, 0);

				TileBase tile = decorationsTilemap.GetTile(tilePosition);

				if (tile != null && tile.ToString().Contains("cash"))
				{
					queueStart = decorationsTilemap.CellToWorld(tilePosition) + new Vector3(0.5f, -0.25f, 0);
				}
			}
		}
	}

	public void AddInteractionPrompt()
	{
		Destroy(interaction1Ref);
		Destroy(interaction2Ref);

		// To Prevent duplicate spots
		bool rhythmSpot = false;
		bool toppingSpot = false;

		// Map over the tilemap, grab the corners of the counters and then put the prefab on it's bottom left corner
		BoundsInt bounds = furnishingTilemap.cellBounds;
		TileBase[] allTiles = furnishingTilemap.GetTilesBlock(bounds);

		for (int x = bounds.xMin; x < bounds.xMax; x++) {
			for (int y = bounds.yMin; y < bounds.yMax; y++) {
				Vector3Int tilePosition = new Vector3Int(x, y, 0);

				TileBase tile = furnishingTilemap.GetTile(tilePosition);

				if (tile != null) {
					if (tile.ToString().Contains("rhythm") && !rhythmSpot)
					{
						// Add the interactioin prompt for rhythm game
						Vector3 worldPos = furnishingTilemap.CellToWorld(tilePosition) + new Vector3(1f, 0, 0);
						cookingSpot1 = new Vector3(worldPos.x, worldPos.y - 1, worldPos.z);
						orderManager.rhythmPos = worldPos;

						interaction1Ref = Instantiate(rhythmPrefab, worldPos, Quaternion.identity);
						rhythmSpot = true;
					} else if (tile.ToString().Contains("topping") && !toppingSpot) 
					{
						// Interactoin prompt for topping game
						Vector3 worldPos = furnishingTilemap.CellToWorld(tilePosition) + new Vector3(1f, 0, 0);
						cookingSpot2 = new Vector3(worldPos.x, worldPos.y - 1, worldPos.z);
						orderManager.toppingPos = worldPos;

						interaction2Ref = Instantiate(toppingPrefab, worldPos, Quaternion.identity);
						toppingSpot = true;
					}
				}
			}
		}
	}

	private IEnumerator SpawnCustomers()
	{
		while (true)
		{
			if (waitingQueue.CanAddCustomer())
			{
				CreateAndAddCustomer();
				doorbell.Play();
			}

			int customerSpawn;

			switch (customerRate)
			{
				case 0:
 					customerSpawn = Random.Range(40, 60);
					break;
				case 1:
 					customerSpawn = Random.Range(35, 55);
					break;
				case 2:
 					customerSpawn = Random.Range(25, 45);
					break;
				case 3:
 					customerSpawn = Random.Range(15, 35);
					break;
				case 4:
 					customerSpawn = Random.Range(8, 18);
					break;
				default:
 					customerSpawn = Random.Range(40, 60);
					break;
			}

			customerSpawn = Random.Range(10, 15);

			yield return new WaitForSeconds(customerSpawn);
		}
	}

	private void CreateAndAddCustomer() {
		GameObject customerObject = new GameObject("Customer");

		customerObject.transform.position = spawnPosition;

		Customer customer = customerObject.AddComponent<Customer>();

		customer.timerPrefab = timerPrefab;

		customer.effect = effect;
		customer.bronzeCoin = bronzeCoin;
		customer.silverCoin = silverCoin;
		customer.goldCoin = goldCoin;
		customer.trophyCoin = trophyCoin;
		customer.entrancePosition = spawnPosition;
		customer.angryFace = angryFace;
		customer.happyFace = happyFace;
		customer.furnishingTilemap = furnishingTilemap;
		customer.bobaTile = bobaTile;
		customer.decorationsTilemap = decorationsTilemap;
		customer.customerLevelUpgrade = customerLevelUpgrade;

		customer.Initialize(waitingQueue.CanAddCustomer() ? waitingQueue.customerList.Count : 0);

		customers.Add(customer);

		waitingQueue.AddCustomer(customer);
	}

	// Rhythm stuff
	public void RegisterCircle()
	{
		totalSpawned++;
		UpdateUI(GetRhythmAccuracy());
	}

	public void RegisterHit()
	{
		totalClicked++;
		currentStreak++;
		UpdateUI(GetRhythmAccuracy());
	}

	public void RegisterMiss()
	{
		currentStreak = 0;
		UpdateUI(GetRhythmAccuracy());
	}

	public void UpdateTopping(int score)
	{
		toppingScore = score;

		UpdateUI(GetToppingAccuracy());
	}

	public float GetToppingAccuracy()
	{
		return (float)toppingScore / toppingScoreFiveStar;
	}
	
	public float GetRhythmAccuracy()
	{
		return (float)totalClicked / totalSpawned;
	}

	public void UpdateUI(float accuracy)
	{
		int count = 0;
		int starCount = Mathf.Clamp(Mathf.FloorToInt(accuracy * 10), 0, 10);

		GameObject stars = GameObject.Find("stars");

		foreach (Transform child in stars.transform)	
		{
			SpriteRenderer starSprite = child.GetComponent<SpriteRenderer>();
			if (starSprite != null)
			{
				starSprite.enabled = false;
			}
		}

		foreach (Transform child in stars.transform)	
		{
			if (count >= starCount) break;

			SpriteRenderer starSprite = child.GetComponent<SpriteRenderer>();

			if (starSprite != null)
			{
				starSprite.enabled = true;
				count++;
			}
		}
	}

	public void UpgradeRestaurant()
	{
		if (restaurantLevel < 4)
		{
			restaurantLevel += 1;

			for (int i = 0; i < restaurantLevel; i++) {
				restaurants[i].SetActive(false);
			}

			restaurants[restaurantLevel].SetActive(true);

			furnishingTilemap = GameObject.Find("Furnishing").GetComponent<Tilemap>();
			decorationsTilemap = GameObject.Find("Decorations").GetComponent<Tilemap>();

			// Clear all customers too
			seatManager.FindAllSeats();

			waitingQueue.ClearQueue();
			GetWaitingQueueStart();
			UpdateWaitingQueue();

			orderManager.ClearOrders();

			AddInteractionPrompt();

			foreach (Customer customer in customers)
			{
				Destroy(customer.gameObject);
			}

			customers.Clear();
		};
	}

	public void UpgradeCustomer()
	{
		customerLevelUpgrade++;
	}

	public void UpgradeAds()
	{
		customerRate++;
	}

	public void AddChef()
	{
		// REMEMBER TO UPDATE EMPLOYEE WHEN UPGRADING RESTAURANTS
		// REMEMBER TO UPDATE EMPLOYEE'S WHEN UPGRADING THEM

		GameObject employee = Instantiate(employeePrefab, cookingSpot1, Quaternion.identity);
		Employee employeeScript = employee.GetComponent<Employee>();

		employeeScript.cookingSpot1 = cookingSpot1;
		employeeScript.cookingSpot2 = cookingSpot2;
		employeeScript.orderTime = employeeOrderTime;
		employeeScript.employeeRevenue = employeeRevenue;

		employees.Add(employeeScript);
	}

	public void UpgradeEmployeeSpeed()
	{
		employeeOrderTime -= 10;
		UpgradeChef();
	}

	public void UpgradeEmployeeRevenue()
	{
		employeeRevenue += 1;
		UpgradeChef();
	}

	public void UpgradeChef()
	{
		foreach (Employee employee in employees)
		{
			employee.orderTime = employeeOrderTime;
			employee.employeeRevenue = employeeRevenue;
		}
	}

	public void UpgradeQueue()
	{
		waitingQueueSize += 1;

		UpdateWaitingQueue();
	}
}
