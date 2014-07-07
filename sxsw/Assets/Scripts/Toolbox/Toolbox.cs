using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Toolbox. - This class was written for another game. So it might not make much sense. But it works.
/// </summary>
public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	public BackgroundMusic bgMusic;
	public PlayVideosInFolder videoBackground;
	
	public GameObject p1, p2;
	
	public GameObject winnerTitle;
	
	public List<GameObject> p1Lives, p2Lives;
	public Texture greenCatDead, purpleCatDead, greenWinner, purpleWinner;
	
	
	public bool playerWonDisabled = false;
	
	
	private int p1Wins = 0, p2Wins = 0;
	
	void Awake () {
		Time.captureFramerate = 30;
	}
	
	
	IEnumerator Popup(GameObject go, AudioSource sound, bool disable)
	{
		yield return new WaitForSeconds(1f);
		go.SetActive(true);
//		startFight.animation.Play("Appear");
		
		if(null != sound)
			sound.Play();
		
		yield return new WaitForSeconds(5f);
		//		go.animation.Play("Disappear"); // not playing
		if(disable)
			go.SetActive(false);	
	}
	
	
	public void EnablePlayerWon()
	{
		playerWonDisabled = false;	
	}
	
	public void PlayerWon(Players winner)
	{
		if(playerWonDisabled == true)
			return;
		
		playerWonDisabled = true;
		
		Camera.main.GetComponent<CameraShake>().ShakeCamera(100f);
		
		if(winner == Players.P1)
		{
				
			p1Wins ++;
			
			p2Lives[p1Wins - 1].renderer.material.mainTexture = purpleCatDead;	
			p2Lives[p1Wins - 1].animation.Play();		
			
			
			
			// Best out of 3
			if(p1Wins == 3)
			{
				// P1 WIn
				Debug.Log("P1 won the game"); 
				playerWonDisabled = true;  
				winnerTitle.renderer.material.mainTexture = greenWinner;
				winnerTitle.SetActive(true);
				StartCoroutine(Popup(winnerTitle, winnerTitle.GetComponent<AudioSource>(), false)); 
				
				StartCoroutine(ReloadLevel());		    	
			}		    
			else
			{
//				StartCoroutine(Popup(wonRound, applause, true)); 
				SetBothPlayersInvincible(7f);				    
			}
		}
		else if(winner == Players.P2)
		{
			p2Wins ++;
			p1Lives[p2Wins - 1].renderer.material.mainTexture = greenCatDead;	
			p1Lives[p2Wins - 1].animation.Play();	
			
			
			
			// Best out of 3
			if(p2Wins == 3)
			{
				// P2 Win
				Debug.Log("P2 won the game");
				playerWonDisabled = true; 
				winnerTitle.renderer.material.mainTexture = purpleWinner;
				winnerTitle.SetActive(true);
				StartCoroutine(Popup(winnerTitle, winnerTitle.GetComponent<AudioSource>(), false)); 
//				StartCoroutine(Popup(wonAllRounds, applause, false)); 

				StartCoroutine(ReloadLevel());		    	
				
			}
			else
			{
//				StartCoroutine(Popup(wonRound, applause, true));
				
				SetBothPlayersInvincible(7f);		    
			}
			
			//			applause.Play();	
		}
		
		if(p1Wins < 3 && p2Wins < 3)
			Invoke("EnablePlayerWon", 3f);
			
		
	}
	
	IEnumerator ReloadLevel()
	{
		yield return new WaitForSeconds(5f);
		Application.LoadLevel(Application.loadedLevel);
	
	}
	

	
	
	void SetBothPlayersInvincible(float time)
	{	
//		p1.GetComponent<Health>().BeInvincible(time);
//		p2.GetComponent<Health>().BeInvincible(time);	
	}
	
	

	
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			//			Debug.Log("Player 1 won");
			PlayerWon(Players.P1);		
		}
		
		if(Input.GetKeyDown(KeyCode.O))
		{
			PlayerWon(Players.P2);	
		}	
	}
		
		
		
	
	
	
	
	
	// (optional) allow runtime registration of global objects
	//	static public T RegisterComponent<T> () {
	//		return Instance.GetOrAddComponent<T>();
	//	}
}