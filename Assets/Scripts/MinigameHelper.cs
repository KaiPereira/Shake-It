using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHelper : MonoBehaviour
{
    public IEnumerator FadeStarsToMiddle(
        float endingDuration,
        Transform middlePosition
        )
	{
		float duration = endingDuration;
		float elapsedTime = 0f;

		GameObject stars = GameObject.Find("stars");

		Vector3 startPosition = stars.transform.position;
		Color startColor = Color.white;

		while (elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			stars.transform.position = Vector3.Lerp(startPosition, middlePosition.position, t);

			if (stars.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
			{
				sr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1, 0, t));
			}

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		stars.transform.position = middlePosition.position;
		if (stars.TryGetComponent<SpriteRenderer>(out SpriteRenderer finalSr))
		{
			finalSr.color = new Color(startColor.r, startColor.g, startColor.b, 0);
		}
	}


	public IEnumerator FadeOutMusic(
        float endingDuration,
        AudioSource audioSource
        )
	{
		float startVolume = audioSource.volume;

		float elapsedTime = 0f;
		float duration = endingDuration;

		while (elapsedTime < duration)
		{
			audioSource.volume = Mathf.Lerp(startVolume, 0, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		audioSource.volume = 0;
		audioSource.Stop();
	}

    public IEnumerator FadeInCompletionMusic(float endingDuration, AudioSource completionAudioSource)
	{
		float elapsedTime = 0f;
		float duration = endingDuration;

		while (elapsedTime < duration)
		{
			completionAudioSource.volume = Mathf.Lerp(0, 1, elapsedTime / duration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		completionAudioSource.volume = 1;
		completionAudioSource.Stop();
	}
}
