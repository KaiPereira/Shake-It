using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
	public AudioClip[] sounds;
	private AudioSource source;

	// Start is called before the first frame update
	void Start()
	{
		source = GetComponent<AudioSource>();

		source.volume = 0.5f;
		source.clip = sounds[Random.Range(0, sounds.Length)];
		source.Play();
	}

	public void SwitchSound()
	{
		source.clip = sounds[Random.Range(0, sounds.Length)];
		source.Play();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
