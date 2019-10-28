using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Basic math operations. 
 */
public static class MyStaticLibrary {

	public static System.Random random;

	static MyStaticLibrary() {
		random = new System.Random();
	}

	/**
	 * https://stackoverflow.com/questions/218060/random-gaussian-variables
	 */
	public static float[] NextRandomGaussianArray(int n = 1, float mean = 0, float standardDeviation = 1) {
		float[] randomUniformArray1 = NextRandomUniformArray(n);
		float[] randomUniformArray2 = NextRandomUniformArray(n);

		float[] randomNextGaussianArray = new float[n];
		for (int i = 0; i < n; i++) {
			randomNextGaussianArray[i] = mean + standardDeviation * Mathf.Sqrt(-2f * Mathf.Log(randomUniformArray1[i])) * Mathf.Sin(2f * Mathf.PI * randomUniformArray2[i]);
		}
		return randomNextGaussianArray;
	}

	public static float[] NextRandomUniformArray(int n=1, float min=0, float max=1) {
		float[] randomNextUniformArray = new float[n];
		for (int i=0; i<n; i++) {
			randomNextUniformArray[i] = min + (max - min) * (float)random.NextDouble();
		}
		return randomNextUniformArray;
	}

	/**
	 * Input: new float[] {-0.6093569, 0.4891967, -0.9337962, -0.3492237, 0.7063468}
	 * Output: -0.9337962
	 */
	public static float maxMagnitudeFloat(float[] floatArray) {
		float maxMagnitudeFloat = 0;
		for (int f=0; f<floatArray.Length; f++) {
			maxMagnitudeFloat = Mathf.Abs(floatArray[f]) > Mathf.Abs(maxMagnitudeFloat) ? floatArray[f] : maxMagnitudeFloat;
		}
		return maxMagnitudeFloat;
	}

	public static float GetDistance(GameObject gameObject1, GameObject gameObject2) {
		return GetDistance(gameObject1.transform.position, gameObject2.transform.position);
	}

	public static float GetDistance(Transform transform1, Transform transform2) {
		return GetDistance(transform1.position, transform2.position);
	}

	public static float GetDistance(Vector3 position1, Vector3 position2) {
		Vector3 direction_1_2 = position2 - position1;
		return direction_1_2.magnitude;
	}

}
