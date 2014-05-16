using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	private float shakePower = 0;
	private float shakeSpeed = 12;
	private float shakeDecay = 4;
	private Perlin perlin;

	void Start()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		
		perlin = new Perlin();
	}

	public void ShakeCamera(float power)
	{
		shakePower = power;
	}

	void Update()
	{
		shakePower = Mathf.Lerp(shakePower, 0, Time.deltaTime * shakeDecay);

		transform.localPosition = new Vector3(perlin.Noise(Time.time * shakeSpeed),
		                                      perlin.Noise((Time.time * shakeSpeed) + 100),
		                                      0) * shakePower;
	}
}
