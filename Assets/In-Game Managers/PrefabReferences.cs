using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Allows me to instantiate generic prefabs. 
 */
public class PrefabReferences : MonoBehaviour {

	public static PrefabReferences prefabReferences;
	public GameObject block;
	public GameObject circleSmall;
	public GameObject circleMedium;
	public GameObject circleLarge;
	public GameObject circleSmall2;//mass = 0.037845 in inspector
	public GameObject circleMedium2;
	public GameObject circleLarge2;

	public Sprite circleMediumX;
	public Sprite circleMediumY;
	public Sprite circleMediumYL;
	public Sprite circleMediumYT;
	public Sprite squareMediumX;
	public Sprite squareMediumY;
	public Sprite squareMediumYL;
	public Sprite squareMediumYT;

	public GameObject bullet;

	public GameObject dashGameObject;

	public GameObject m9GameObject;
	public GameObject financialItemGameObject;

	protected virtual void Awake() {
		if (prefabReferences == null) {//like a singleton
			//DontDestroyOnLoad (gameObject);
			prefabReferences = this;
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
