using System.Collections;
using UnityEngine;

public class SceneReferences : MonoBehaviour {

	public static SceneReferences sceneReferences_;
	public GameObject environmentGameObject_;
	public GameObject agentsGameObject_;
	public GameObject itemsGameObject_;
	public GameObject projectilesGameObject_;

	protected virtual void Awake() {
		if (sceneReferences_ == null) {  //like a singleton
			sceneReferences_ = this;
		} else {
			Destroy(gameObject);
		}
	}

	protected virtual void Start() {

	}

	protected virtual void Update() {

	}
}