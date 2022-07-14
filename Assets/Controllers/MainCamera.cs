using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public static MainCamera mainCamera_;
	public Transform playerTransform_;

	protected void Awake() {
		if (mainCamera_ == null) {
			//DontDestroyOnLoad (gameObject);
			mainCamera_ = this;
		} else {
			Destroy(gameObject);
		}
	}

	protected void Update() {
		transform.position = new Vector3(playerTransform_.position.x, playerTransform_.position.y, -10f);
	}
}
