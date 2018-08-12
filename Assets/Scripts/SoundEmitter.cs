using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
	[SerializeField]
	private float PitchMinRange = 0.6f;
	[SerializeField]
	private float PitchMaxRange = 1.5f;

	private AudioSource m_AudioSource;
	
	void Start ()
	{
		m_AudioSource = gameObject.AddComponent<AudioSource>();
		m_AudioSource.playOnAwake = false;
		m_AudioSource.spread = 10.0f;
	}

	public void PlaySound(AudioClip clip)
	{
		m_AudioSource.pitch = Random.Range(PitchMinRange, PitchMaxRange);
		m_AudioSource.PlayOneShot(clip);
	}
}
