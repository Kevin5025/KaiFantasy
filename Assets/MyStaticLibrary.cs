using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Basic math operations. 
 */
public static class MyStaticLibrary {

	public static float GetDistance(GameObject gameObject1, GameObject gameObject2) {
		Vector3 direction_1_2 = gameObject2.transform.position - gameObject1.transform.position;
		return direction_1_2.magnitude;
	}

	public static float GetDistance(Vector2 vector2_1, Vector2 vector2_2) {
		Vector3 direction_1_2 = vector2_2 - vector2_1;
		return direction_1_2.magnitude;
	}

}
