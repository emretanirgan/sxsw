using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class PlayVideosInFolder : MonoBehaviour 
{

	public string path = "C:/TrippyVideos/";
	public int videoIndex = 0;
	public bool shouldLoop = true;
	
	private FileInfo[] vids;
	
	private WWW www;
	private MovieTexture movTex;
	
	// Use this for initialization
	void Start () {
		// Getting list of all files in folder
		DirectoryInfo dir = new DirectoryInfo(path);
		vids = dir.GetFiles("*.ogg");
		
		Debug.Log("Vids length = " + vids.Length); 
		
		foreach(FileInfo vid in vids)
		{
			Debug.Log(vid.FullName);
		}
		
		// Play first video file
		if(vids.Length > 0)
			PlayVideo(videoIndex);
		else
		{
			Debug.LogError("No videos found to playback");
		}
	
//		renderer.material.mainTexture = movieTex;
//		movieTex.Play();
//		movieTex.loop = shouldLoop;
	}
	
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.V))
		{
			PlayNextVideo();		
		}
	}
	

	

	
	public void PlayNextVideo()
	{
		videoIndex = (videoIndex + 1) % vids.Length;
		PlayVideo(videoIndex);
	}
	
	public void PlayRandomVideo()
	{
		if(vids.Length > 0)
		{
			videoIndex = Random.Range(0, vids.Length + 1);
			PlayVideo(videoIndex);		
		}
		else
		{
			Debug.LogError("No videos found to playback");
		}
	}
	
	void PlayVideo(int indexOfVid)
	{
		StartCoroutine(CoPlayVideo(indexOfVid));	
	}
	
	IEnumerator CoPlayVideo(int indexOfVid)
	{
		if(null != movTex)
			movTex.Stop();
//		System.GC.Collect();
		
		
//		WWW www = new WWW ("http://www.unity3d.com/webplayers/Movie/sample.ogg"); 
//		WWW www =  new WWW("file://C:/Users/Kiran/PA Game Studio/Kinect for Windows v2/Unity/-The+Music+Scene--HD.web");
//		new WWW ("file://" + vids[indexOfVid].FullName);

		string wwwArg = "file://" + vids[indexOfVid].FullName;
		
//		print("Playing " + wwwArg);

//		WWW www =  new WWW("file://C:/testVid.web");
		
		www =  new WWW(wwwArg);
		
		
		yield return www;
		 
		movTex = (MovieTexture) www.movie; 
		
		renderer.material.mainTexture = movTex;
		movTex.loop = true; 
		movTex.Play();
		
		
		
		audio.clip = movTex.audioClip;
		audio.loop = true;
		audio.Play();
		
		
		
	}
}

