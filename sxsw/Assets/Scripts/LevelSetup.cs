﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


public class LevelSetup : MonoBehaviour {
	public const int posSize = 1000;


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
	                                     float[] objWidth, float[] boundingBox, float[] objBGR, 
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
	int threshold = 150; //range 0-255
	float[] objPosX;
	float[] objPosY;
	float[] objHeight;
	float[] objWidth;
	float[] boundingBox;
	float[] objBGR;
	UnityContourPoints [] unityContourPts;
	bool scanned = false;
	
	public Camera mainCam;
	public Transform ground;

	public GameObject player;
	public GameObject basic_platform;
	public GameObject start_platform;
	public GameObject exit_platform;
	public GameObject spring_platform;
	float levelDepth;



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
		boundingBox = new float[4];


		unityContourPts = new UnityContourPoints[100];

		
		for(int i = 0; i < 100; i++)
		{
			objPosX[i] = objPosY[i] = objHeight[i] = objWidth[i] = objBGR[i] = 0;

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

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("scan") && !scanned)
		{
			scanned = true;
			scanAs.Play();
			int sizeNum = detectShape(minRadius, maxRadius, threshold, objPosX, objPosY, objHeight, objWidth, boundingBox, objBGR, true, unityContourPts);


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
				newPlatform.transform.localScale = new Vector3(objWidth[i] / (boundingBox[2] - boundingBox[0]) * 160, objHeight[i] / (boundingBox[3] - boundingBox[1]) * 120, 20);

			}

			// place player on top of starting platform
			GameObject[] startingPlatforms = GameObject.FindGameObjectsWithTag("start");
			if(startingPlatforms.Length >= 1)
			{
				print ("instantiating");
				player = Instantiate(player) as GameObject;
				//Vector3 startingPos = startingPlatforms[0].transform.position + new Vector3(0, startingPlatforms[0].transform.localScale.y + player.transform.localScale.y * 0.5f * player.GetComponent<CharacterController>().height, 0);
				Vector3 startingPos =  startingPlatforms[0].transform.position + new Vector3(0, 5, 0);
				player.transform.position = startingPos;
			}
			else
			{
				print ("We have to have one and only one starting platform");
			}
			
			Destroy(GameObject.Find("White Screen"));
			Destroy(GameObject.Find("Black Block"));

			print(sizeNum);

			for(int i = 0; i < sizeNum; i++)
			{
				Mesh m;
				print("unity contours " + unityContourPts[i].size);

				CreateMesh meshScript = gameObject.GetComponent<CreateMesh>();
				m = meshScript.extrudeMesh(unityContourPts[i].posX, unityContourPts[i].posY);

				gameObject.AddComponent(typeof(MeshRenderer));
				MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
				filter.mesh = m;
			}



		}
		
	}
}
