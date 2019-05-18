using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages collisions. 
 */
public class LayersManager : MonoBehaviour {

	public static LayersManager layersManager;

	public int blueEntityLayer;
	public int blueProjectileLayer;
	public int redEntityLayer;
	public int redProjectileLayer;
	public int brownEntityLayer;
	public int brownProjectileLayer;
	public int[] entityLayerArray;
	public int[] projectileLayerArray;
	public int[] layerArray;

	void Awake() {
		if (layersManager == null) {//like a singleton
									//DontDestroyOnLoad (gameObject);
			layersManager = this;
		} else { //if (menuColors != null)
			Destroy(gameObject);
		}
	}
	
	void Start() {
		blueEntityLayer = 8;
		blueProjectileLayer = 9;
		redEntityLayer = 10;
		redProjectileLayer = 11;
		brownEntityLayer = 12;
		brownProjectileLayer = 13;
		entityLayerArray = new int[] { blueEntityLayer, redEntityLayer, brownEntityLayer };
		projectileLayerArray = new int[] { blueProjectileLayer, redProjectileLayer, brownProjectileLayer };

		for (int p=0; p<projectileLayerArray.Length; p++) {
			for (int e=0; e<entityLayerArray.Length; e++) {
				Physics2D.IgnoreLayerCollision(projectileLayerArray[p], entityLayerArray[e]);
			}
			for (int p2=0; p2<=p; p2++) {
				Physics2D.IgnoreLayerCollision(projectileLayerArray[p], projectileLayerArray[p2]);
			}
		}

		//blue
		//Physics2D.IgnoreLayerCollision(blueProjectileLayer, blueProjectileLayer, true);

		//red
		//Physics2D.IgnoreLayerCollision(redProjectileLayer, redProjectileLayer, true);

		//brown
		//Physics2D.IgnoreLayerCollision(brownProjectileLayer, brownProjectileLayer, true);
	}

	public int GetTeamEntityLayer(Spirit.Affinity affinity) {
		if (affinity == Spirit.Affinity.BLUE) {
			return blueEntityLayer;
		} else if (affinity == Spirit.Affinity.RED) {
			return redEntityLayer;
		} else if (affinity == Spirit.Affinity.BROWN) {
			return brownEntityLayer;
		} else {
			return -1;
		}
	}

	public int GetTeamProjectileLayer(Spirit.Affinity affinity) {
		if (affinity == Spirit.Affinity.BLUE) {
			return blueProjectileLayer;
		} else if (affinity == Spirit.Affinity.RED) {
			return redProjectileLayer;
		} else if (affinity == Spirit.Affinity.BROWN) {
			return brownProjectileLayer;
		} else {
			return -1;
		}
	}
	
}
