using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages collisions. 
 */
public class LayersManager : MonoBehaviour {

	public static LayersManager layersManager;
	public static int numLayers;

	public int blueEntityLayer;
	public int blueProjectileLayer;
	public int redEntityLayer;
	public int redProjectileLayer;
	public int brownEntityLayer;
	public int brownProjectileLayer;

	public int itemLayer;
	public int handItemLayer;  // ignore this layer for now

	public int entityTriggerLayer;
	public int projectileTriggerLayer;

	public int newProjectileLayer;

	public int wallProjectileLayer;
	public int wallEntityLayer;
	public int wallLayer;

	public int[] entityLayerArray;
	public int[] projectileLayerArray;
	public int[] wallLayerArray;

	public int[] allLayerArray;
	public int[] allLayerMaskArray;

	public int entityLayerMask;
	public int projectileLayerMask;
	public int allLayerMask;

	static LayersManager() {
		numLayers = 32;
	}

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

		itemLayer = 20;
		handItemLayer = 21;

		entityTriggerLayer = 22;
		projectileTriggerLayer = 23;

		newProjectileLayer = 25;

		wallProjectileLayer = 29;  // projectile can go through
		wallEntityLayer = 30;  // entity can go through
		wallLayer = 31; // none can go through

		entityLayerArray = new int[] { blueEntityLayer, redEntityLayer, brownEntityLayer };
		projectileLayerArray = new int[] { blueProjectileLayer, redProjectileLayer, brownProjectileLayer, newProjectileLayer };
		wallLayerArray = new int[] { wallProjectileLayer, wallEntityLayer, wallLayer };
		allLayerArray = new int[] {
			blueEntityLayer, blueProjectileLayer, redEntityLayer, redProjectileLayer, brownEntityLayer, brownProjectileLayer, 
			itemLayer, handItemLayer, entityTriggerLayer, projectileTriggerLayer, newProjectileLayer,
			wallProjectileLayer, wallEntityLayer, wallLayer,
		};

		allLayerMaskArray = new int[numLayers];
		for (int li=0; li<allLayerMaskArray.Length; li++) {
			allLayerMaskArray[li] = 1 << li;
		}

		int entityLayerMask = GetLayerMask(entityLayerArray);
		int projectileLayerMask = GetLayerMask(projectileLayerArray);
		int allLayerMask = GetLayerMask(allLayerArray);
		
		SetLayerCollisionMask(entityTriggerLayer, entityLayerMask);  // entityTriggers only collide with entities
		SetLayerCollisionMask(projectileTriggerLayer, projectileLayerMask);  // projectileTriggers only collide with projectiles

		// projectiles go through entities, projectiles
		for (int p = 0; p < projectileLayerArray.Length; p++) {
			SetLayerCollisionMask(projectileLayerArray[p], Physics2D.GetLayerCollisionMask(projectileLayerArray[p]) & ~(entityLayerMask | projectileLayerMask));
		}

		SetLayerCollisionMask(itemLayer, GetLayerMask(itemLayer) | GetLayerMask(handItemLayer) | GetLayerMask(wallLayerArray));  // items only collide with themselves and walls
		SetLayerCollisionMask(handItemLayer, GetLayerMask(itemLayer) | GetLayerMask(handItemLayer) | GetLayerMask(wallLayerArray));  // items only collide with themselves and walls
		SetLayerCollisionMask(wallProjectileLayer, Physics2D.GetLayerCollisionMask(wallProjectileLayer) & ~GetLayerMask(projectileLayerArray));
		SetLayerCollisionMask(wallEntityLayer, Physics2D.GetLayerCollisionMask(wallEntityLayer) & ~GetLayerMask(entityLayerArray));
	}

	private int GetLayerMask(int[] layerArray) {
		int layerMask = 0;
		for (int li=0; li<layerArray.Length; li++) {
			// layerMask |= (1 << layerArray[li]);
			layerMask |= GetLayerMask(layerArray[li]);
		}
		return layerMask;
	}

	private int GetLayerMask(int layer) {
		int layerMask = 1 << layer;
		return layerMask;
	}

	/*
	 * Physics2D.SetLayerCollisionMask seems to have a bug as it does not affect the transpose of the layer collision matrix
	 */
	private void SetLayerCollisionMask(int layer, int layerMask) {
		BitArray layerMaskBitArray = new BitArray(new int[1] { ~layerMask });
		for (int li=0; li<numLayers; li++) {
			bool ignore = layerMaskBitArray[li];
			Physics2D.IgnoreLayerCollision(layer, li, ignore);
		}
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
