using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public int currentStreak;
	public int totalClicked;
	public int totalSpawned;

	public int toppingScore;
	public int toppingScoreFiveStar = 12;

	public int customerSpawn = 20;

	public AudioSource doorbell;
	private Vector3 spawnPosition = new Vector3(-24, -5, 0);

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
		List<Vector3> waitingQueuePositionList = new List<Vector3>();
		Vector3 entrancePosition = new Vector3(-24, -1);

		float positionSize = 1f;

		for (int i = 0; i < 5; i++) {
			waitingQueuePositionList.Add(entrancePosition + new Vector3(0f, -i * positionSize, 0f));
		}

		waitingQueue.Initialize(waitingQueuePositionList);

		StartCoroutine(SpawnCustomers());
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

		customer.Initialize(waitingQueue.CanAddCustomer() ? waitingQueue.customerList.Count : 0);

		waitingQueue.AddCustomer(customer);
	}

	// Rhythm stuff
	public void RegisterCircle()
	{
		totalSpawned++;
		float accuracy = (float)totalClicked / totalSpawned;
		UpdateUI(accuracy);
	}

	public void RegisterHit()
	{
		totalClicked++;
		currentStreak++;
		float accuracy = (float)totalClicked / totalSpawned;
		UpdateUI(accuracy);
	}

	public void RegisterMiss()
	{
		currentStreak = 0;
		float accuracy = (float)totalClicked / totalSpawned;
		UpdateUI(accuracy);
	}

	public void UpdateTopping(int score)
	{
		toppingScore = score;

		float accuracy = (float)toppingScore / toppingScoreFiveStar;

		UpdateUI(accuracy);
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
