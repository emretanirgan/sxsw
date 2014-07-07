
#include "ColorBasics.h"


int main()
{
	float* objPosX = new float[100];
	float* objPosY = new float[100];
	float* objHeight = new float[100];
	float* objWidth = new float[100];
	float* objHue = new float[100];
	float* objAngle = new float[100];
	float* boudingBox = new float[4];

	for(int i = 0;i < 100; i++)
	{
		objPosX[i] = objPosX[i] = objHeight[i] = objWidth[i] = 0;
	}

	for(int i = 0; i < 4; i++)
	{
		boudingBox[i] = 0;
	}

	ContourPoints* contourPts = new ContourPoints[100];

	static CColorBasics colorKinect;
	colorKinect.maxRadius = 120;
	colorKinect.minRadius = 40;
	colorKinect.threshhold = 74;
	colorKinect.debugMode = true;
	colorKinect.CreateFirstConnected();
	int shapeNum = 0;

	while(1){
		Sleep(100);
		if ( WAIT_OBJECT_0 == WaitForSingleObject(colorKinect.m_hNextColorFrameEvent, 0) )
		{
			colorKinect.ProcessColor();			
			//colorKinect.ShapeBoundingbox(objPosX, objPosY, objHeight, objWidth, objAngle, shapeNum, boudingBox, objHue, contourPts);
			colorKinect.ShapeBoundingboxWithArea(objPosX, objPosY, objHeight, objWidth, objAngle, shapeNum, boudingBox, objHue, 1636, 2300);

			break;
		}
	}
	system("pause");
	return 0;
	
}