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
	private bool rhythmGameActive;

	private LevelLoader levelLoader;

	// Start is called before the first frame update
	void Start()
	{
		promptUI.SetActive(false);
		levelLoader = FindObjectOfType<LevelLoader>();
	}

	// Update is called once per frame
	void Update()
	{
		if (inRange && Input.GetKeyDown(KeyCode.E) && !rhythmGameActive)
		{
			if (trigger == 0) {
				StartRhythmGame();
			} else if (trigger == 1) {
				levelLoader.LoadNextLevel("ToppingGame");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			promptUI.SetActive(true);
			inRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			promptUI.SetActive(false);
			inRange = false;
		}
	}

	void StartRhythmGame() {
		rhythmGameActive = true;
		levelLoader.LoadNextLevel("RhythmGame");
	}
}