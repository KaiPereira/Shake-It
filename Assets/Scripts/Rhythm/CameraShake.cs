using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public float shakeDuration = 0.1f;
	public float shakeMagnitude = 0.2f;

	private Vector3 originalPosition;

	// Start is called before the first frame update
	void Start()
	{
		originalPosition = transform.position;
	}

	public void TriggerShake()
	{
		StartCoroutine(Shake());
	}

	private IEnumerator Shake()
	{
		float elapsed = 0f;

		while (elapsed < shakeDuration)
		{
			Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
			transform.position = originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

			elapsed += Time.deltaTime;
			yield return null;
		}

		transform.position = originalPosition;
	}
}
