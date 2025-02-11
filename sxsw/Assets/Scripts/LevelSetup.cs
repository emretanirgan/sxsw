using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


public class LevelSetup : MonoBehaviour {
	public const int posSize = 1000;
	public Material blockMaterial;

	public GameObject spawnPos1, spawnPos2;
	public GameObject cat1, cat2;

	//the data to pass to dll
	[StructLayout(LayoutKind.Sequential)]
	public struct UnityContourPoints
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = posSize)]
		public float[] posX;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = posSize)]
		public float[] posY;
		public int size;//size of these points
	}




	[DllImport("ShapeDetectionUnity")]
	public static extern int detectShape(float minRadius, float maxRadius, int threshold, 
	                                     float[] objPosX, float[] objPosY, float[] objHeight, 
	                                     float[] objWidth, float[] objAngle, float[] boundingBox, float[] objBGR, 
	                                     bool debugging,  
	                                     [Out][MarshalAs(UnmanagedType.LPArray)]UnityContourPoints[] unityContourPts); 
	

//	int EXPORT_API detectShapeWithArea(float minArea, float maxArea, int threshold,
//	                                   float* objPosX, float* objPosY, float* objHeight, float* objWidth, float* objAngle, float* boundingBox, float* objHue,
//	                                   bool debugMode)	                                                                          	                                                                          
	[DllImport("ShapeDetectionUnity")]
	public static extern int detectShapeWithArea(float minArea, float maxArea, int threshold, 
	                                     float[] objPosX, float[] objPosY, float[] objHeight, 
	                                     float[] objWidth, float[] objAngle, float[] boundingBox, float[] objBGR, 
	                                     bool debugging);  
	                                    
	                                     
	
//	[DllImport("ShapeDetectionUnity")]
//	public static extern int detectShape2(float minRadius, float maxRadius, int threshold, 
//	                                     float[] objPosX, float[] objPosY, float[] objHeight, 
//	                                     float[] objWidth, float[] objAngle, float[] boundingBox, float[] objBGR, 
//	                                     bool debugging,  
//	                                     [Out][MarshalAs(UnmanagedType.LPArray)]UnityContourPoints[] unityContourPts);  
	
	
	[DllImport("ShapeDetectionUnity")]
	public static extern void testStruct([Out][MarshalAs(UnmanagedType.LPArray)] UnityContourPoints[] unityContourPts); 
	/*public static extern int detectShape(float minRadius, float maxRadius, int threshold, 
	                                     float[] objPosX, float[] objPosY, float[] objHeight, 
	                                     float[] objWidth, float[] boundingBox, float[] objBGR, 
	                                     bool debugging,  
	                                     ref UnityContourPoints[] unityContourPts);*/



	float minRadius = 20;
	float maxRadius = 140;
	public int threshold = 130; //range 0-255
	float[] objPosX;
	float[] objPosY;
	float[] objHeight;
	float[] objWidth;
	float[] boundingBox;
	float[] objBGR;
	float[] objAngle;
	UnityContourPoints [] unityContourPts;
	bool scanned = false;
	bool scanStart = false;
	
	public Camera mainCam;
	public Transform ground;

	public GameObject player;
	public GameObject basic_platform;
	public GameObject videoBgChangePlatform;	
	public GameObject spring_platform;
	float levelDepth;

	float timer = 0;
	float videoTime;

//	public float hSliderValue = 0.0F ;
//	
	public float xPosMultiplier = 1f, yPosMultiplier = 1f, widthMultiplier = 1f, heightMultiplier = 1f;
	
	public List<GameObject> allPlatforms;
	public float realWidth = 640f, realHeight = 480f;
	public bool applyAdjustments = false;
	
	public float realTopLeftX, realTopLeftY;
	
	public float xScaleMultiplier = 1f, yScaleMultiplier = 1f;
	public float xOffset, yOffset;
	
	public int minArea = 1636, maxArea = 2300; // minimum and maximum area of blocks that can be detected
	
	private bool showGui = true;
	//public string stringToEdit = "Enter threshold value here";
	//calibration stuff
	void OnGUI() {
		if(!showGui)
			return;
	
		//stringToEdit = GUI.TextField(new Rect(10, 10, 200, 20), stringToEdit, 25);
		GUI.Label (new Rect(25, 5, 250, 30), "Threshold: " + threshold);
		threshold = (int)GUI.HorizontalSlider(new Rect(25, 25, 100, 30), threshold, 0, 255);
		
		GUI.Label (new Rect(25, 35, 250, 30), "X Offset : " + xOffset);
		xOffset = GUI.HorizontalSlider(new Rect(25, 55, 100, 30), xOffset, 0.0F, 250f);
		
		GUI.Label (new Rect(25, 65, 250, 30), "Y Offset : " + yOffset);
		yOffset = GUI.HorizontalSlider(new Rect(25, 85, 100, 30), yOffset, 0.0F, 250f);

		
		GUI.Label (new Rect(25, 95, 250, 30), "X Scale Multiplier: " + xScaleMultiplier);
		xScaleMultiplier = GUI.HorizontalSlider(new Rect(25, 115, 100, 30), xScaleMultiplier, 1f, 5f);
		
		GUI.Label (new Rect(25, 125, 250, 30), "Y Scale Multiplier: " + yScaleMultiplier);
		yScaleMultiplier = GUI.HorizontalSlider(new Rect(25, 145, 100, 30), yScaleMultiplier, 1f, 5f);
		
		// other params
		GUI.Label (new Rect(Screen.width - 200, 5, 250, 30), "Real Width: " + realWidth);
		realWidth = GUI.HorizontalSlider(new Rect(Screen.width - 200, 25, 200, 30), realWidth, 0, 640);
		
		GUI.Label (new Rect(Screen.width - 200, 35, 250, 30), "Real Height:" + realHeight);
		realHeight = GUI.HorizontalSlider(new Rect(Screen.width - 200, 55, 200, 30), realHeight, 0.0F, 480f);
		
		GUI.Label (new Rect(Screen.width - 200, 65, 250, 30), "Real Top Left X: " + realTopLeftX);
		realTopLeftX = GUI.HorizontalSlider(new Rect(Screen.width - 200, 85, 200, 30), realTopLeftX, 0.0F, 640f);
		
		
		GUI.Label (new Rect(Screen.width - 200, 95, 250, 30), "Real Top Left Y: " + realTopLeftY);
		realTopLeftY = GUI.HorizontalSlider(new Rect(Screen.width - 200, 115, 200, 30), realTopLeftY, 0f, 480f);
	
		
		GUI.Label (new Rect(Screen.width - 200, 145, 250, 30), "Min Area: " + minArea);
		minArea = (int)GUI.HorizontalSlider(new Rect(Screen.width - 200, 165, 200, 30), minArea, 1000, 2000);
		
		GUI.Label (new Rect(Screen.width - 200, 185, 250, 30), "Max Area: " + maxArea);
		maxArea = (int)GUI.HorizontalSlider(new Rect(Screen.width - 200, 205, 200, 30), maxArea, 2000, 5000);

		
		
		
		
		
	}
	
	void ReadPlayerPrefs()
	{
		threshold = PlayerPrefs.GetInt("threshold", 120);
	
		xOffset = PlayerPrefs.GetFloat("xOffset", 0f);
		yOffset = PlayerPrefs.GetFloat("yOffset", 0f);
		
		xScaleMultiplier = PlayerPrefs.GetFloat("xScaleMultiplier", 2.1f);
		yScaleMultiplier = PlayerPrefs.GetFloat("yScaleMultiplier", 2.1f);
		
		realWidth = PlayerPrefs.GetFloat("realWidth", 582f);
		realHeight = PlayerPrefs.GetFloat("realHeight", 327f);
		
		realTopLeftX = PlayerPrefs.GetFloat("realTopLeftX", 29f);
		realTopLeftY = PlayerPrefs.GetFloat("realTopLeftY", 82f);
		
		minArea = PlayerPrefs.GetInt("minArea", 1449);
		maxArea = PlayerPrefs.GetInt("realHeight", 3000);		
	
	}
	
	
	void WritePlayerPrefs()
	{
		PlayerPrefs.SetInt("threshold", threshold);
	
		PlayerPrefs.SetFloat("xOffset", xOffset);
		PlayerPrefs.SetFloat("yOffset", yOffset);
		
		PlayerPrefs.SetFloat("xScaleMultiplier", xScaleMultiplier);
		PlayerPrefs.SetFloat("yScaleMultiplier", yScaleMultiplier);
		
		PlayerPrefs.SetFloat("realWidth", realWidth);
		PlayerPrefs.SetFloat("realHeight", realHeight);
		
		
		PlayerPrefs.SetFloat("realTopLeftX", realTopLeftX);
		PlayerPrefs.SetFloat("realTopLeftY", realTopLeftY);
		
		PlayerPrefs.SetInt("minArea", minArea);
		PlayerPrefs.SetInt("maxArea", maxArea);	
		
	}
	
	void ApplyAdjustments()
	{
		for(int i = 0; i < allPlatforms.Count; i++)
		{
//			Vector3 screenPosition = new Vector3((realWidth - objPosX[i]) / (realWidth - 0) * Screen.width, 
//			                                     Screen.height - (objPosY[i] - 0) / (realHeight - 0) * Screen.height, 
//			                                     levelDepth); 
			                                     
			float realObjPosX = objPosX[i] - realTopLeftX;
			float realObjPosY = objPosY[i] - realTopLeftY;                                      
			Vector3 screenPosition = new Vector3((realWidth - realObjPosX) / (realWidth) * (xPosMultiplier != 1 ? xPosMultiplier : Screen.width), 
//			                                     Screen.height - (objPosY[i] - 0) / (realHeight - 0) * Screen.height, 
			                                     Screen.height - ((realObjPosY / realHeight) * Screen.height),
			                                     levelDepth); 
			
			// Adding offset from center of screen                                     
			Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
			
			
			int xOffsetDirection = screenPosition.x > screenCenter.x ? 1 : -1;
			int yOffsetDirection = screenPosition.y > screenCenter.y ? 1 : -1;
			screenPosition = new Vector3(screenPosition.x + (xOffsetDirection * xOffset),
			                             screenPosition.y + (yOffsetDirection * yOffset), 0);
				
			
			
			
			Debug.Log(i + ". x = " + objPosX[i] + " y = " + objPosY[i] + " screenPos = " + screenPosition);
			//print ("previous: " + (Screen.currentResolution.width - (objPosX[i] - boundingBox[0]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
			//print ("now: " + ((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
			allPlatforms[i].transform.position = mainCam.ScreenToWorldPoint(screenPosition);
			
			
			
			allPlatforms[i].transform.position = new Vector3(allPlatforms[i].transform.position.x, allPlatforms[i].transform.position.y, spawnPos1.transform.position.z);
			allPlatforms[i].transform.eulerAngles = new Vector3(allPlatforms[i].transform.eulerAngles.x, allPlatforms[i].transform.eulerAngles.y, objAngle[i]);
			//				newPlatform.transform.localScale = new Vector3(objWidth[i] / (boundingBox[2] - boundingBox[0]) * 160, objHeight[i] / (boundingBox[3] - boundingBox[1]) * 120, 20);
//			allPlatforms[i].transform.localScale = new Vector3(objWidth[i] / (640) * 160, objHeight[i] / (480) * 120, 20);
			allPlatforms[i].transform.localScale = new Vector3((objWidth[i] / realWidth) * Screen.width, (objHeight[i] / realHeight) * Screen.height , 20);
			Vector3 screenDimensionsInUnityUnits = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
//			screenDimensionsInUnityUnits = new Vector3(screenDimensionsInUnityUnits.x * xScaleMultiplier, screenDimensionsInUnityUnits.y * yScaleMultiplier, 0);
//			screenDimensionsInUnityUnits = Quaternion.AngleAxis(objAngle[i], Vector3.forward) * screenDimensionsInUnityUnits;
			float screenWidthInUnityUnits = screenDimensionsInUnityUnits.x * xScaleMultiplier;
			float screenHeightInUnityUnits = screenDimensionsInUnityUnits.y * yScaleMultiplier;
			
			
			
			allPlatforms[i].transform.localScale = new Vector3((objWidth[i] / realWidth) * screenWidthInUnityUnits, (objHeight[i] / realHeight) * screenHeightInUnityUnits , 20);
		
			
			allPlatforms[i].layer = LayerMask.NameToLayer ("Collisions");
			
		
		
		}
	
	}

	public AudioSource scanAs;
	public AudioClip sacnClip;

	// Use this for initialization
	void Start () {


		ReadPlayerPrefs();
		applyAdjustments = true;

//		scanAs = (AudioSource)gameObject.AddComponent<AudioSource>();
//		scanAs.clip = sacnClip;
		
		
		objPosX = new float[100];
		objPosY = new float[100];
		objHeight = new float[100];
		objWidth = new float[100];
		objBGR = new float[100];
		objAngle = new float[100];
		boundingBox = new float[4];




		unityContourPts = new UnityContourPoints[100];

		
		for(int i = 0; i < 100; i++)
		{
			objPosX[i] = objPosY[i] = objHeight[i] = objWidth[i] = objBGR[i] = objAngle[i] = 0;

			unityContourPts[i].posX = new float [posSize];
			unityContourPts[i].posY = new float [posSize];

			for(int j = 0; j < posSize; j++)
				unityContourPts[i].posX[j] = unityContourPts[i].posY[j] = 0;
			unityContourPts[i].size = 0;
		}
		boundingBox[0] = 0;
		boundingBox[1] = 0;
		boundingBox[2] = 640;
		boundingBox[3] = 480;
		

		levelDepth = Mathf.Abs((mainCam.transform.position - ground.position).z);
		//levelDepth = (player.transform.TransformPoint (player.transform.position)).z/2;
		//levelDepth = 5;

	}
	
	IEnumerator StartBgMusic()
	{
		Toolbox.Instance.bgMusic.GetComponent<AudioSource>().Stop();
		yield return new WaitForSeconds(2.5f);
		Toolbox.Instance.bgMusic.PlayNextMusicClip();
		

	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.G))
		{
			showGui = !showGui;
		}
		
		if(Input.GetKeyDown(KeyCode.C))
		{
			PlayerPrefs.DeleteAll();
		}
		
		if(Input.GetKeyDown(KeyCode.V))
		{
			WritePlayerPrefs();
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Break();
		
		}
		
		
	
		if(Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		
		if(applyAdjustments)
			ApplyAdjustments();

		if(Input.GetButtonDown("scan") && !scanned)
		{
			showGui = false;
//			threshold = (int)hSliderValue;
			videoTime = Time.time + 6;
			
			scanStart = true;
			//scanned = animateScan();
			scanned = true;
//			scanAs.Play();
			GetComponent<AudioSource>().Play();
			StartCoroutine(StartBgMusic());
			
			int sizeNum = 0;
//			int sizeNum = detectShape(minRadius, maxRadius, threshold, objPosX, objPosY, objHeight, objWidth, objAngle, boundingBox, objBGR, true, unityContourPts);
//			sizeNum = detectShapeWithArea(minArea, maxArea, threshold, objPosX, objPosY, objHeight, objWidth, objAngle, boundingBox, objBGR, true);


			//testStruct(unityContourPts);

			for(int i = 0; i < 4; i++)
			{
				//print("boudingbox " + i + " " + boundingBox[i]);
			}

			for(int i = 0; i < sizeNum; i++)
			{
				
				GameObject newPlatform;

				//print ("objWidth: " + objWidth[i]);
				//print ("objHeight: " + objHeight[i]);
				//print ("objHue: " + objBGR[3*i+2] + ", " + objBGR[3*i+1] + ", " + objBGR[3*i]);

				float primeValue = Mathf.Max (objBGR[3*i+2], Mathf.Max(objBGR[3*i+1], objBGR[3*i]));
				float averageValue = (objBGR[3*i+2] + objBGR[3*i+1] + objBGR[3*i]) / 3;
				float blackThreshold = 20;

				//print("Color Value: " + (objBGR[3*i+2] + ", " +  objBGR[3*i+1] + ", " +  objBGR[3*i]));
//				if((objBGR[3*i+2] - averageValue) < blackThreshold && (objBGR[3*i+1] - averageValue) < blackThreshold && (objBGR[3*i] - averageValue) < blackThreshold)
//					newPlatform = Instantiate(videoBgChangePlatform) as GameObject;
//				else if(objBGR[3*i+2] == primeValue)
//					newPlatform = Instantiate(basic_platform) as GameObject;
//				else if(objBGR[3*i+1] == primeValue)
//					newPlatform = Instantiate(exit_platform) as GameObject;
//				else 
//					newPlatform = Instantiate(spring_platform) as GameObject;

				float randomValue = UnityEngine.Random.value;
				
				if(randomValue < 0.6f)
					newPlatform = Instantiate(basic_platform) as GameObject;
				else if (randomValue < 0.8f)
					newPlatform = Instantiate(spring_platform) as GameObject;
				else 
					newPlatform = Instantiate(videoBgChangePlatform) as GameObject;

				//GameObject newPlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);
				//newPlatform.renderer.material.color = new Color(objHue[3*i+2]/255, objHue[3*i+1]/255, objHue[3*i]/255);
				Vector3 screenPosition = new Vector3((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width, 
				                                     Screen.currentResolution.height - (objPosY[i] - boundingBox[1]) / (boundingBox[3] - boundingBox[1]) * Screen.currentResolution.height, 
				                                     levelDepth); 

				// instead of Screen.width & Screen.height... Screen.currenRes.width & height
				                                     
//				Vector3 screenPosition = new Vector3((640 - objPosX[i]) / (640 - 0) * Screen.width, 
//				                                     Screen.height - (objPosY[i] - 0) / (480 - 0) * Screen.height, 
//				                                     levelDepth); 
				
				                                     
				Debug.Log(i + ". x = " + objPosX[i] + " y = " + objPosY[i] + " screenPos = " + screenPosition);
				//print ("previous: " + (Screen.currentResolution.width - (objPosX[i] - boundingBox[0]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
				//print ("now: " + ((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
				newPlatform.transform.position = mainCam.ScreenToWorldPoint(screenPosition);
				newPlatform.transform.position = new Vector3(newPlatform.transform.position.x, newPlatform.transform.position.y, spawnPos1.transform.position.z);
				newPlatform.transform.eulerAngles = new Vector3(newPlatform.transform.eulerAngles.x, newPlatform.transform.eulerAngles.y, objAngle[i]);
				newPlatform.transform.localScale = new Vector3(objWidth[i] / (boundingBox[2] - boundingBox[0]) * 160, objHeight[i] / (boundingBox[3] - boundingBox[1]) * 120, 20);
//				newPlatform.transform.localScale = new Vector3(objWidth[i] / (640) * 160, objHeight[i] / (480) * 120, 20);
//				newPlatform.transform.localScale = new Vector3((objWidth[i] / 640) * Screen.width, (objHeight[i] / 480) * Screen.height , 20);
				

				newPlatform.layer = LayerMask.NameToLayer ("Collisions");
				
				allPlatforms.Add(newPlatform);
			}
			
			// New positioning method
			ApplyAdjustments();

			// place player on top of starting platform
//			GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("start");
//			if(startingPlatforms.Length >= 1)
//			{
				print ("instantiating");
//				player = Instantiate(player) as GameObject;

				player = Instantiate(cat1, spawnPos1.transform.position, Quaternion.identity) as GameObject;
				Instantiate(cat2, spawnPos2.transform.position, Quaternion.identity);
				//Vector3 startingPos = startingPlatforms[0].transform.position + new Vector3(0, startingPlatforms[0].transform.localScale.y + player.transform.localScale.y * 0.5f * player.GetComponent<CharacterController>().height, 0);
//				Vector3 startingPos =  startingPlatforms[0].transform.position + new Vector3(0, 5, 0);
//				player.transform.position = startingPos;
//			}
//			else
//			{
//				print ("We have to have one and only one starting platform");
//			}

			Destroy(GameObject.Find("Grey Screen"));
			//Destroy(GameObject.Find("Black Block"));

			print(sizeNum);
			/*
			for(int i = 0; i < sizeNum; i++)
			{


				Mesh m;
				print("unity contours " + unityContourPts[i].size);
				for (int j=0; j<unityContourPts[i].size; j++)
					print ("contour x: " + unityContourPts[i].posX[j]);
				//unityContourPts[i].s
				print ("center x: " + objPosX[i]);

				CreateMesh meshScript = gameObject.GetComponent<CreateMesh>();
				m = meshScript.extrudeMesh(unityContourPts[i].posX, unityContourPts[i].posY, unityContourPts[i].size, new Vector3(objPosX[i], objPosY[i], levelDepth));

				//GameObject platform = new GameObject();
				//platform = Instantiate (basic_platform) as GameObject;
				//platform.GetComponent<MeshFilter>().mesh = m;


				GameObject go = new GameObject();
				go.AddComponent(typeof(MeshRenderer));
				go.GetComponent<MeshRenderer>().material = blockMaterial;
				MeshFilter filter = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
				go.renderer.material.color = new Color(objBGR[3*i+2]/255, objBGR[3*i+1]/255, objBGR[3*i]/255);
				filter.mesh = m;
				go.AddComponent (typeof(MeshCollider));
				go.GetComponent<MeshCollider>().sharedMesh = m;
				go.layer = LayerMask.NameToLayer ("Collisions");
				Vector3 oldPosition = filter.transform.position;

				Vector3 screenPosition = new Vector3((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width, 
				                                     Screen.currentResolution.height - (objPosY[i] - boundingBox[1]) / (boundingBox[3] - boundingBox[1]) * Screen.currentResolution.height, 
				                                     player.transform.position.zlevelDepth); 
//				//print ("previous: " + (Screen.currentResolution.width - (objPosX[i] - boundingBox[0]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
//				//print ("now: " + ((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
				go.transform.position = mainCam.ScreenToWorldPoint(screenPosition);
				go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, player.transform.position.z);
				//filter.transform.localPosition = new Vector3(filter.transform.position.x, 12, filter.transform.position.z);
				//go.transform.localScale = new Vector3(objWidth[i] / (boundingBox[2] - boundingBox[0]) * 160, objHeight[i] / (boundingBox[3] - boundingBox[1]) * 120, 20);
				go.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);

				//Array.Reverse (cloneMesh.triangles);
				//cloneMesh.triangles.
				/*for(int j=0; j<cloneMesh.normals.Length; j++){
					cloneMesh.normals[j] = (-1* cloneMesh.normals[j]); 
				}*/
				/*for(int k=0; k<filter.mesh.vertices.Length; k++)
				{
					filter.mesh.vertices[k] -= go.transform.position;
					filter.mesh.vertices[k].x *= go.transform.localScale.x;
					filter.mesh.vertices[k].y *= go.transform.localScale.y;
					filter.mesh.vertices[k].z *= go.transform.localScale.z;
				}

			}
			}*/
		}
			if(scanStart && !scanned){
				scanned = animateScan();
			}
	}

	bool animateScan()
	{
		scanStart = false;
		GameObject.FindGameObjectWithTag("scanPlane").GetComponent<ScanVideoScript>().playScanAnim();
		timer += Time.deltaTime;
		if(Time.time > videoTime){
			return true;
		}
		else {
			return false;
		}

	}
}
