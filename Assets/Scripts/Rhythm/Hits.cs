using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hits : MonoBehaviour
{
	private SpriteRenderer sr;
	private Color OriginalColor;
	private Color FadedColor;

	[HideInInspector]
	public Color color = Color.blue;

	public float speed = 0.5f;
	public float circleLifetime = 1.5f;
	public float fullLifetime = 10.0f;

	private static SpriteRenderer bobaSprite;
	private static bool isFlipped;

	private GameManager gameManager;
	private bool wasClicked = false;

	void Start()
	{
		GetRandomColor();

		gameManager = FindObjectOfType<GameManager>();

		gameManager.RegisterCircle();

		bobaSprite = GameObject.FindGameObjectWithTag("Boba").GetComponent<SpriteRenderer>();

		sr = GetComponent<SpriteRenderer>();
		OriginalColor = color;
		FadedColor = new Color(OriginalColor.r, OriginalColor.g, OriginalColor.b, 0f);
		StartCoroutine(AlphaLerp());

		Invoke(nameof(CheckForMiss), circleLifetime);
	}

	public IEnumerator AlphaLerp()
	{
		for (float i = 0; i < circleLifetime ; i+= Time.deltaTime)
		{
			sr.color = Color.Lerp(OriginalColor, FadedColor, i / circleLifetime);
			yield return null;
		}
	}

	public void OnMouseDown() {
		wasClicked = true;

		FlipBoba();

		ShowStreak();

		gameManager.RegisterHit();
	}

	void FlipBoba()
	{
		isFlipped = !isFlipped;
		bobaSprite.flipX = isFlipped;
	}

	void CheckForMiss()
	{
		if (!wasClicked)
		{
			gameManager.RegisterMiss();
			Destroy(gameObject);
		}
	}

	void ShowStreak()
	{
		GameObject textObject = new GameObject("StreakText");

		TextMeshPro textMesh = textObject.AddComponent<TextMeshPro>();

		textMesh.text = "x" + gameManager.currentStreak.ToString();
		textMesh.fontSize = 5;
		textMesh.color = color;
		textMesh.alignment = TextAlignmentOptions.Center;

		textObject.transform.position = transform.position;

		StartCoroutine(MoveUpAndFade(textObject));
	}

	public IEnumerator MoveUpAndFade(GameObject textObject)
	{
		TextMeshPro textMesh = textObject.GetComponent<TextMeshPro>();
		float elapsedTime = 0f;
		float duration = 1.0f;
		Vector3 startPos = textObject.transform.position;
		Vector3 endPos = startPos + Vector3.up * 0.5f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			textObject.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
			textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1f - (elapsedTime / duration)); // Fade out
			yield return null;
		}

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		Destroy(gameObject);
		Destroy(textObject);
	}

	void GetRandomColor()
	{
		int colorInt = Random.Range(0, 4);

		switch (colorInt)
		{
			case 0:
				color = Color.cyan;
				break;
			case 1:
				color = Color.red;
				break;
			case 2:
				color = Color.green;
				break;
			case 3:
				color = Color.yellow;
				break;
		}
	}
}
