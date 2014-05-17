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
	public GameObject start_platform;
	public GameObject exit_platform;
	public GameObject spring_platform;
	float levelDepth;

	public float hSliderValue = 0.0F;
	//public string stringToEdit = "Enter threshold value here";
	//calibration stuff
	void OnGUI() {
		//stringToEdit = GUI.TextField(new Rect(10, 10, 200, 20), stringToEdit, 25);
		GUI.Label (new Rect(25, 5, 250, 30), "Threshold: " + hSliderValue);
		hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0.0F, 255.0F);

	}

	public AudioSource scanAs;
	public AudioClip sacnClip;

	// Use this for initialization
	void Start () {




		scanAs = (AudioSource)gameObject.AddComponent<AudioSource>();
		scanAs.clip = sacnClip;
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
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		if(Input.GetButtonDown("scan") && !scanned)
		{
			threshold = (int)hSliderValue;

			scanStart = true;
			//scanned = animateScan();
			//scanned = true;
			scanAs.Play();
			int sizeNum = detectShape(minRadius, maxRadius, threshold, objPosX, objPosY, objHeight, objWidth, objAngle, boundingBox, objBGR, true, unityContourPts);


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
				if((objBGR[3*i+2] - averageValue) < blackThreshold && (objBGR[3*i+1] - averageValue) < blackThreshold && (objBGR[3*i] - averageValue) < blackThreshold)
					newPlatform = Instantiate(start_platform) as GameObject;
				else if(objBGR[3*i+2] == primeValue)
					newPlatform = Instantiate(basic_platform) as GameObject;
				else if(objBGR[3*i+1] == primeValue)
					newPlatform = Instantiate(exit_platform) as GameObject;
				else 
					newPlatform = Instantiate(spring_platform) as GameObject;

				//GameObject newPlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);
				//newPlatform.renderer.material.color = new Color(objHue[3*i+2]/255, objHue[3*i+1]/255, objHue[3*i]/255);
				Vector3 screenPosition = new Vector3((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width, 
				                                     Screen.currentResolution.height - (objPosY[i] - boundingBox[1]) / (boundingBox[3] - boundingBox[1]) * Screen.currentResolution.height, 
				                                     levelDepth); 
				//print ("previous: " + (Screen.currentResolution.width - (objPosX[i] - boundingBox[0]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
				//print ("now: " + ((boundingBox[2] - objPosX[i]) / (boundingBox[2] - boundingBox[0]) * Screen.currentResolution.width));
				newPlatform.transform.position = mainCam.ScreenToWorldPoint(screenPosition);
				newPlatform.transform.position = new Vector3(newPlatform.transform.position.x, newPlatform.transform.position.y, player.transform.position.z);
				newPlatform.transform.eulerAngles = new Vector3(newPlatform.transform.eulerAngles.x, newPlatform.transform.eulerAngles.y, objAngle[i]);
				newPlatform.transform.localScale = new Vector3(objWidth[i] / (boundingBox[2] - boundingBox[0]) * 160, objHeight[i] / (boundingBox[3] - boundingBox[1]) * 120, 20);
				newPlatform.layer = LayerMask.NameToLayer ("Collisions");
			}

			// place player on top of starting platform
			GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("start");
			if(startingPlatforms.Length >= 1)
			{
				print ("instantiating");
//				player = Instantiate(player) as GameObject;

				player = Instantiate(cat1, spawnPos1.transform.position, Quaternion.identity) as GameObject;
				Instantiate(cat2, spawnPos2.transform.position, Quaternion.identity);
				//Vector3 startingPos = startingPlatforms[0].transform.position + new Vector3(0, startingPlatforms[0].transform.localScale.y + player.transform.localScale.y * 0.5f * player.GetComponent<CharacterController>().height, 0);
//				Vector3 startingPos =  startingPlatforms[0].transform.position + new Vector3(0, 5, 0);
//				player.transform.position = startingPos;
			}
			else
			{
				print ("We have to have one and only one starting platform");
			}

			Destroy(GameObject.Find("White Screen"));
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
		return true;
		GUITexture whiteScreen = GameObject.FindGameObjectWithTag ("whitescreen").GetComponent<GUITexture>();
		GUITexture horizScan = GameObject.FindGameObjectWithTag ("horizscan").GetComponent<GUITexture>();
		GUITexture vertScan = GameObject.FindGameObjectWithTag ("vertscan").GetComponent<GUITexture>();
		if (horizScan.pixelInset.y > 0){
			horizScan.pixelInset = new Rect(horizScan.pixelInset.x, horizScan.pixelInset.y - 4, horizScan.pixelInset.width, horizScan.pixelInset.height);
		}
		else if(vertScan.pixelInset.x < 1500){
			vertScan.pixelInset = new Rect(vertScan.pixelInset.x + 10, vertScan.pixelInset.y, vertScan.pixelInset.width, vertScan.pixelInset.height);
		}
		else{
			vertScan.pixelInset = new Rect(vertScan.pixelInset.x + 20, vertScan.pixelInset.y, vertScan.pixelInset.width, vertScan.pixelInset.height);
			horizScan.pixelInset = new Rect(horizScan.pixelInset.x+20, horizScan.pixelInset.y, horizScan.pixelInset.width, horizScan.pixelInset.height);
			whiteScreen.pixelInset = new Rect(whiteScreen.pixelInset.x+20, whiteScreen.pixelInset.y, whiteScreen.pixelInset.width, whiteScreen.pixelInset.height);
		}
		return (horizScan.pixelInset.x > 5000);
	}
}
