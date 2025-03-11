using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatDetector : MonoBehaviour
{
	public AudioSource audioSource;
	private float audioFadeDuration = 2f;
	public GameObject circlePrefab;
	public Transform spawnArea; // Assign SpawnArea object here
	public float sensitivity = 1.2f;
	public float timeBetweenBeats = 0.2f;
	public float musicLength = 5f;
	private bool hasEnded = false;

	private float lastPeak = 0f;
	private float[] samples = new float[512];

	public Camera mainCamera;
	public Color originalColor;
	public Color beatColor;

	public float fadeDuration = 0.5f;

	private CameraShake cameraShake;

	private BobaPulse bobaPulse;

	public GameObject stars;
	public Transform middlePosition;
	public AudioSource completionAudioSource;

	public float endingDuration = 4f;

	public Animator bigBobaAnimation;
	private float bigBobaAnimationSpeed = 0.5f;
	public SpriteRenderer bigBoba;
	private bool gameStarted = false;

	private LevelLoader levelLoader;
	private OrderManager orderManager;
	private MinigameHelper minigameHelper;
	private GameManager gameManager;

	void Start()
	{
		cameraShake = FindObjectOfType<CameraShake>();
		bobaPulse = FindObjectOfType<BobaPulse>();
		minigameHelper = FindObjectOfType<MinigameHelper>();
		gameManager = FindObjectOfType<GameManager>();

		bigBoba.enabled = false;

		bigBobaAnimation.speed = bigBobaAnimationSpeed;

		levelLoader = FindObjectOfType<LevelLoader>();
		orderManager = FindObjectOfType<OrderManager>();

		StartCoroutine(EndSequence());
		StartCoroutine(WaitForAnimation());
		StartCoroutine(FadeInAudio());
	}

	void Update()
	{
		if (gameStarted)
		{
			audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
			float sum = 0;

			for (int i = 0; i < samples.Length; i++)
			{
				sum += samples[i];
			}

			if (sum > lastPeak * sensitivity && Time.time - lastPeak > timeBetweenBeats)
			{
				SpawnCircle();
				StartCoroutine(FadeBackgroundColor());
				cameraShake.TriggerShake();
				bobaPulse.Pulse();
				lastPeak = Time.time;
			}
		}
	}

	void SpawnCircle()
	{
		Vector3 randomPos = GetRandomPosition();
		Instantiate(circlePrefab, randomPos, Quaternion.identity);
	}

	Vector3 GetRandomPosition()
	{
		if (spawnArea == null) return Vector3.zero;

		Vector3 areaSize = spawnArea.localScale; 
		Vector3 areaPosition = spawnArea.position;

		float randomX = Random.Range(areaPosition.x - areaSize.x / 2, areaPosition.x + areaSize.x / 2);
		float randomY = Random.Range(areaPosition.y - areaSize.y / 2, areaPosition.y + areaSize.y / 2);

		return new Vector3(randomX, randomY, 0);
	}

	private IEnumerator FadeBackgroundColor()
	{
		float timeElapsed = 0f;
		while (timeElapsed < fadeDuration)
		{
			mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, beatColor, timeElapsed / fadeDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		mainCamera.backgroundColor = beatColor; // Ensure the color is exactly the target color

		timeElapsed = 0f;
		while (timeElapsed < fadeDuration)
		{
			mainCamera.backgroundColor = Color.Lerp(beatColor, originalColor, timeElapsed / fadeDuration);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		mainCamera.backgroundColor = originalColor; // Ensure the color is exactly the original color
	}

	private IEnumerator EndSequence()
	{
		yield return new WaitForSeconds(musicLength);

		if (hasEnded) yield break;

		hasEnded = true;

		StartCoroutine(minigameHelper.FadeOutMusic(endingDuration, audioSource));

		completionAudioSource.volume = 0;
		completionAudioSource.Play();
		StartCoroutine(minigameHelper.FadeInCompletionMusic(endingDuration, completionAudioSource));

		StartCoroutine(FadeOutBigBoba());
		StartCoroutine(minigameHelper.FadeStarsToMiddle(endingDuration, middlePosition));

		orderManager.UpdateOrderScore(gameManager.GetRhythmAccuracy());
		orderManager.CompleteStep();

		yield return new WaitForSeconds(4);
		StartCoroutine(levelLoader.UnloadAdditiveScene());
	}

	private IEnumerator FadeOutBigBoba()
	{
		GameObject bigBoba = GameObject.Find("big_boba");

		SpriteRenderer sr = bigBoba.GetComponent<SpriteRenderer>();

		Color startColor = sr.color;
		float elapsedTime = 0f;
		float duration = endingDuration;

		while (elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			sr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1, 0, t));

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		sr.color = new Color(startColor.r, startColor.g, startColor.b, 0);
	}

	private IEnumerator WaitForAnimation()
	{
		while (bigBobaAnimation.GetCurrentAnimatorStateInfo(0).IsName("BigBobaEntrance"))
		{
			yield return null;
		}

		while (bigBobaAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
			yield return null;
		}


		bigBobaAnimation.gameObject.SetActive(false);
		bigBoba.enabled = true;
		gameStarted = true;
	}

	private IEnumerator FadeInAudio()
	{
		float elapsedTime = 0f;
		audioSource.volume = 0;
		audioSource.Play();
		
		while (elapsedTime < audioFadeDuration)
		{
			audioSource.volume = Mathf.Lerp(0, 1, elapsedTime / audioFadeDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		audioSource.volume = 1;
	}
}

