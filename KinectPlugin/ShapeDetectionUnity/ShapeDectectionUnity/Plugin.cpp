#include<Windows.h>
#include <array>
#include "ColorBasics.h"

#if _MSC_VER // this is defined when compiling with Visual Studio
#define EXPORT_API __declspec(dllexport) // Visual Studio needs annotating exported functions with this
#else
#define EXPORT_API // XCode does not need annotating exported functions, so define is empty
#endif


extern "C"
{
	//struct used to get contour positions
	typedef struct
	{
		float posX[POINTNUM];
		float posY[POINTNUM];
		int size;
	} UnityContourPoints;

	
	void copytoC(UnityContourPoints unityContourPt[], ContourPoints contourPt[], int objSize)
	{
		for(int i = 0; i < objSize; i++)
		{
			for(int j = 0; j < POINTNUM; j++)
			{
				contourPt[i].posX[j] = unityContourPt[i].posX[j];
				contourPt[i].posY[j] = unityContourPt[i].posY[j];
			}			
		}
	}


	void copytoUnity(ContourPoints contourPt[], UnityContourPoints unityContourPt[], int objSize)
	{
		for(int i = 0; i < objSize; i++)
		{
			for(int j = 0; j < contourPt[i].size; j++)
			{
				unityContourPt[i].posX[j] = contourPt[i].posX[j];
				unityContourPt[i].posY[j] = contourPt[i].posY[j];
			}			
			unityContourPt[i].size = contourPt[i].size;
		}
	}

	static CColorBasics colorKinect;
	int EXPORT_API detectShape(float minRadius, float maxRadius, int threshold, 
		float* objPosX, float* objPosY, float* objHeight, float* objWidth, float* boundingBox, float* objHue,
		bool debugMode, UnityContourPoints* unityContourPts)
	{
		colorKinect.maxRadius = maxRadius;
		colorKinect.minRadius = minRadius;
		colorKinect.threshhold = threshold;
		colorKinect.debugMode = debugMode;
		colorKinect.CreateFirstConnected();
		int shapeNum = 0;


		ContourPoints* contourPts = new ContourPoints[100];

		//copytoC(unityContourPt, contourPt, objSize);

		int countTimes = 0;
		while(1){
			if(countTimes++ == 20);
//				return shapeNum;
			Sleep(100);
			if ( WAIT_OBJECT_0 == WaitForSingleObject(colorKinect.m_hNextColorFrameEvent, 0) )
			{
				colorKinect.ProcessColor();
				colorKinect.ShapeBoundingbox(objPosX, objPosY, objHeight, objWidth, shapeNum, boundingBox, objHue, contourPts);
				break;
			}
		}

		copytoUnity(contourPts, unityContourPts, shapeNum);

		//shapeNum = 10;

		return shapeNum;
	}


	void EXPORT_API testStruct(UnityContourPoints* unityContourPts)
	{ 
		for(int i = 0; i < 100; i++)
		{
			for(int j = 0; j < 100; j++){
				unityContourPts[i].posX[j] = 2;
				unityContourPts[i].posY[j] = 10;
			}
			unityContourPts[i].size = 3;
		}
	}

}