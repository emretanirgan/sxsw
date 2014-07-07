using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour {

	public List<AudioClip> musicClips;
	
	int clipIndex = 0;

	// Use this for initialization
	void Start () {
		PlayMusicClip(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void PlayRandomMusicClip()
	{
		if(musicClips.Count > 0)
		{
			clipIndex = Random.Range(0, musicClips.Count + 1);
			PlayMusicClip(clipIndex);		
		}
		else
		{
			Debug.LogError("No music clips found to playback");
		}
	}
	
	
	public void PlayNextMusicClip()
	{
		clipIndex = (clipIndex + 1) % musicClips.Count;
		PlayMusicClip(clipIndex);
	}
	
	public void PlayMusicClip(int index)
	{
		audio.Stop();
		audio.clip = musicClips[index];
		audio.Play();
		
	}
	
	
}
