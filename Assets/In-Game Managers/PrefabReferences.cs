using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Allows me to instantiate generic prefabs. 
 */
public class PrefabReferences : MonoBehaviour {

	public static PrefabReferences prefabReferences_;
	public GameObject block_;
	public GameObject circleSmall_;
	public GameObject circleMedium_;
	public GameObject circleLarge_;
	public GameObject circleSmall2_;  //mass = 0.037845 in inspector
	public GameObject circleMedium2_;
	public GameObject circleLarge2_;

	public Sprite circleMediumX_;
	public Sprite circleMediumY_;
	public Sprite circleMediumYL_;
	public Sprite circleMediumYT_;
	public Sprite squareMediumX_;
	public Sprite squareMediumY_;
	public Sprite squareMediumYL_;
	public Sprite squareMediumYT_;

	public GameObject bulletPrefab_;

	public GameObject dashPrefab_;

	public GameObject m9Prefab_;
	public GameObject financialItemPrefab_;

	protected virtual void Awake() {
		if (prefabReferences_ == null) {//like a singleton
			//DontDestroyOnLoad (gameObject);
			prefabReferences_ = this;
		} else { //if (menuColors != null)
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	protected virtual void Start() {

	}

	// Update is called once per frame
	protected virtual void Update() {

	}

}
