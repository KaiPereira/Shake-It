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
	public OrderManager orderManager;

	public Sprite angryFace;
	public Sprite happyFace;

	public int currentStreak;
	public int totalClicked;
	public int totalSpawned;

	public int toppingScore;
	public int toppingScoreFiveStar = 12;

	public int customerSpawn = 20;
	public int customerLevelUpgrade = 0;

	public AudioSource doorbell;
	private Vector3 spawnPosition = new Vector3(-24, -5, 0);

	private Tilemap furnishingTilemap;
	private Tilemap decorationsTilemap;
	public TileBase bobaTile;

	public GameObject rhythmPrefab;
	public GameObject toppingPrefab;
	
	private Vector3 queueStart;

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

		AddInteractionPrompt();
		GetWaitingQueueStart();

		List<Vector3> waitingQueuePositionList = new List<Vector3>();

		float positionSize = 1f;

		for (int i = 0; i < 5; i++) {
			waitingQueuePositionList.Add(queueStart + new Vector3(0f, -i * positionSize, 0f));
		}

		waitingQueue.Initialize(waitingQueuePositionList);

		StartCoroutine(SpawnCustomers());
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
						orderManager.rhythmPos = worldPos;

						Instantiate(rhythmPrefab, worldPos, Quaternion.identity);
						rhythmSpot = true;
					} else if (tile.ToString().Contains("topping") && !toppingSpot) 
					{
						// Interactoin prompt for topping game
						Vector3 worldPos = furnishingTilemap.CellToWorld(tilePosition) + new Vector3(1f, 0, 0);
						orderManager.toppingPos = worldPos;

						Instantiate(toppingPrefab, worldPos, Quaternion.identity);
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
			else
			{

			}

			customerSpawn = Random.Range(40, 60);

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
}
