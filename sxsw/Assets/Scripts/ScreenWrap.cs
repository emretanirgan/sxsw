using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

	Renderer[] renderers;
	
	bool isWrappingX = false;
	bool isWrappingY = false;
	
	void Start()
	{
		renderers = GetComponentsInChildren<Renderer>();
//		Debug.Log("renderers length = " + renderers.Length);
	}
	
	bool CheckRenderers()
	{
		foreach(var renderer in renderers)
		{
			// If at least one render is visible, return true
			if(renderer.isVisible)
			{
				return true;
			}
		}
		
		// Otherwise, the object is invisible
//		Debug.Log("Player not visible");
		return false;
	}
	
	void Update()
	{
		DoScreenWrap();	
	}
	
	
	
	void DoScreenWrap()
	{
		var isVisible = CheckRenderers();
		
		if(isVisible)
		{
//			Debug.Log("Is Visible");
			isWrappingX = false;
			isWrappingY = false;
			return;
		}
		
		if(isWrappingX && isWrappingY) {
			return;
		}
		
		var cam = Camera.main;
		var viewportPosition = cam.WorldToViewportPoint(transform.position);
		var newPosition = transform.position;
		
		if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
		{
//			Debug.Log("wrapping around on x");
			newPosition.x = -newPosition.x;
			
			isWrappingX = true;
		}
		
		if (!isWrappingY && (/*viewportPosition.y > 1 || */ viewportPosition.y < 0))
		{
//			Debug.Log("wrapping around on y");
			newPosition.y = -newPosition.y;
			
			isWrappingY = true;
		}
		
		transform.position = newPosition;
	}
	
	
}
