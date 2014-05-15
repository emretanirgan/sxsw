using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	public Background background;
	public PlayVideosInFolder videoBackground;
	
	void Awake () {
	
	}
	
	
	
	
	
	// (optional) allow runtime registration of global objects
	//	static public T RegisterComponent<T> () {
	//		return Instance.GetOrAddComponent<T>();
	//	}
}