using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager environmentManager;
	
	public Vector2 unitVectorNorthEast;
	public Vector2 unitVectorSouthEast;
	public Vector2 unitVectorSouthWest;
	public Vector2 unitVectorNorthWest;
	public Vector2[] unitVectorDirections;
	public float[] unitVectorMagnitudes;

	protected int wallEntityLayerMask;
	protected int wallLayerMask;
	protected int raycastLayerMask;

	public GameObject boundaryNN;  // set in Inspector
	public GameObject boundaryEE;
	public GameObject boundarySS;
	public GameObject boundaryWW;

	public int minPositionX;
	public int maxPositionX;
	public int minPositionY;
	public int maxPositionY;

	public bool[,,] environmentGraph;  // returns whether (X, Y, V) is a valid vector direction

	void Awake() {
		if (environmentManager == null) {
			environmentManager = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		InitializeUnitVectors();

		wallEntityLayerMask = 1 << LayersManager.layersManager.wallEntityLayer;
		wallLayerMask = 1 << LayersManager.layersManager.wallLayer;
		raycastLayerMask = wallEntityLayerMask | wallLayerMask;

		minPositionX = (int) Mathf.Ceil(boundaryWW.transform.position.x);  // inscribed
		maxPositionX = (int) Mathf.Floor(boundaryEE.transform.position.x);
		minPositionY = (int) Mathf.Ceil(boundarySS.transform.position.y);
		maxPositionY = (int) Mathf.Floor(boundaryNN.transform.position.y);

		InitializeEnvironmentGraph();
	}

	protected void InitializeUnitVectors() {
		unitVectorNorthEast = new Vector2(1f, 1f);  // technically not unit length
		unitVectorSouthEast = new Vector2(1f, -1f);
		unitVectorSouthWest = new Vector2(-1f, -1f);
		unitVectorNorthWest = new Vector2(-1f, 1f);

		unitVectorDirections = new Vector2[] {
			Vector2.zero, Vector2.up, Vector2.right, Vector2.down, Vector2.left,
			unitVectorNorthEast, unitVectorSouthEast, unitVectorSouthWest, unitVectorNorthWest,
		};

		unitVectorMagnitudes = new float[unitVectorDirections.Length];
		for (int v = 0; v<unitVectorMagnitudes.Length; v++) {
			unitVectorMagnitudes[v] = unitVectorDirections[v].magnitude;
		}
	}

	protected void InitializeEnvironmentGraph() {
		environmentGraph = new bool[maxPositionX - minPositionX, maxPositionY - minPositionY, unitVectorDirections.Length];
		for (int indexX = 0; indexX < environmentGraph.GetLength(0); indexX++) {
			for (int indexY = 0; indexY < environmentGraph.GetLength(1); indexY++) {
				Vector2 position = new Vector2(indexX + minPositionX, indexY + minPositionY);
				for (int v = 0; v < environmentGraph.GetLength(2); v++) {
					RaycastHit2D raycastHitDirection = Physics2D.Raycast(position, unitVectorDirections[v], unitVectorMagnitudes[v], raycastLayerMask, 0f, 0f);
					environmentGraph[indexX, indexY, v] = raycastHitDirection.collider != null;
				}
			}
		}
	}
	
	/**
     * Prints environmentGrid on keypress P
     */
	void Update() {
		if (Input.GetKey(KeyCode.P)) {
			for (float positionX = minPositionX; positionX <= maxPositionX; positionX++) {
				for (float positionY = minPositionY; positionY <= maxPositionY; positionY++) {
					Vector2 position = new Vector2(positionX, positionY);
					GameObject circleSmall = Instantiate(PrefabReferences.prefabReferences.circleSmall2, position, Quaternion.identity);

					//float r = 0;
					//float g = 0;
					//float b = 0;
					//for (int v = 0; v<unitVectorDirections.Length; v++) {
					//	RaycastHit2D raycastHitDirection = Physics2D.Raycast(position, unitVectorDirections[v], unitVectorMagnitudes[v], raycastLayerMask, 0f, 0f);
					//	if (raycastHitDirection.collider != null) {
					//		r += 0.1111f;
					//		g += 0.1111f;
					//		b += 0.1111f;
					//	}
					//}
					//circleSmall.GetComponent<SpriteRenderer>().color = new Color(r, g, b);
				}
			}
		}
	}

}
