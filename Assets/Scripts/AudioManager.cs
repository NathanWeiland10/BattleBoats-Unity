// This code was created by Brackeyes (https://www.youtube.com/watch?v=6OT43pvUyfY) and is used to manage all of the audio sources that play in the game and allows the control of audio sources pitch and volume

using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;
	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.Play();
	}

	public void PlayAtPoint(string sound, Vector3 v)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.minDistance = s.minDistance;
		s.source.maxDistance = s.maxDistance;

		s.source.volume = 0f;
		float dist = Math.Abs(FindObjectOfType<AudioListener>().GetComponent<Transform>().position.x - v.x);
		if (dist < s.source.minDistance)
        {
			s.source.volume = 1f;
        }
		else if (dist < s.source.maxDistance && dist > s.source.minDistance)
        {
			s.source.volume = Mathf.Clamp(1-((dist-s.source.minDistance)/(s.source.maxDistance - s.source.minDistance)), 0f, 1f);
		}

		s.source.Play();
	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * 0;
		s.source.pitch = s.pitch * 0;
		s.source.Stop();
	}

}