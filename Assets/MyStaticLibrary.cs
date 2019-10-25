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

	public static float[] NextRandomUniformArray(int n=1, float min=0, float max=1) {
		float[] randomNextUniformArray = new float[n];
		for (int i=0; i<n; i++) {
			randomNextUniformArray[i] = min + (max - min) * (float)random.NextDouble();
		}
		return randomNextUniformArray;
	}

	public static float[] NextRandomGaussianArray(int n=1, float mean=0, float standardDeviation=1) {
		float[] randomUniformArray1 = NextRandomUniformArray(n);
		float[] randomUniformArray2 = NextRandomUniformArray(n);

		float[] randomNextGaussianArray = new float[n];
		for (int i=0; i<n; i++) {
			randomNextGaussianArray[i] = mean + standardDeviation * Mathf.Sqrt(-2f * Mathf.Log(randomUniformArray1[i])) * Mathf.Sin(2f * Mathf.PI * randomUniformArray2[i]);
		}
		return randomNextGaussianArray;
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
		Vector3 direction_1_2 = gameObject2.transform.position - gameObject1.transform.position;
		return direction_1_2.magnitude;
	}

	public static float GetDistance(Vector2 vector2_1, Vector2 vector2_2) {
		Vector3 direction_1_2 = vector2_2 - vector2_1;
		return direction_1_2.magnitude;
	}

}
