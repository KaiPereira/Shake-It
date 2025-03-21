using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionPrompt : MonoBehaviour
{
	public GameObject promptUI;

	// 0 is boba making
	// 1 is the topping game
	public int trigger = 0;

	private bool inRange;

	private LevelLoader levelLoader;
	private OrderManager orderManager;

	// Start is called before the first frame update
	void Start()
	{
		promptUI.SetActive(false);
		levelLoader = FindObjectOfType<LevelLoader>();
		orderManager = FindObjectOfType<OrderManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (inRange && Input.GetKeyDown(KeyCode.E))
		{
			Order currentOrder = orderManager.GetNextOrder();

			if (trigger == 0 && currentOrder.step == 0) {
				levelLoader.LoadNextLevel("RhythmGame");
			} else if (trigger == 1 && currentOrder.step == 1) {
				levelLoader.LoadNextLevel("ToppingGame");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && orderManager.GetNextOrder() != null)
		{
			promptUI.SetActive(true);
			inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && orderManager.GetNextOrder() != null)
		{
			promptUI.SetActive(false);
			inRange = false;
		}
	}
}