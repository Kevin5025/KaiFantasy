using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public static MainCamera mainCamera;
	public Transform playerTransform;

	protected void Awake() {
		if (mainCamera == null) {
			//DontDestroyOnLoad (gameObject);
			mainCamera = this;
		} else {
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	protected void Update() {
		transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
	}
}
