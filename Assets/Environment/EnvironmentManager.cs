using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager environmentManager;

	protected int wallEntityLayerMask;
	protected int wallLayerMask;
	protected int raycastLayerMask;

	public GameObject boundaryNN;  // set in Inspector
	public GameObject boundaryEE;
	public GameObject boundarySS;
	public GameObject boundaryWW;

	public Vector2 unitVectorNorthEast;
	public Vector2 unitVectorSouthEast;
	public Vector2 unitVectorSouthWest;
	public Vector2 unitVectorNorthWest;
	public Vector2[] unitVectorDirections;
	public float[] unitVectorMagnitudes;

	public int minPositionX;
	public int maxPositionX;
	public int minPositionY;
	public int maxPositionY;
	public int maxIndexX;
	public int maxIndexY;

	public bool[,,] environmentGraph;  // returns whether (X, Y, V) is a valid vector direction, the V=0 direction tells us whether the position is clear
	private GameObject[,] circleSmallArray;

	void Awake() {
		if (environmentManager == null) {
			environmentManager = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		wallEntityLayerMask = 1 << LayersManager.layersManager.wallEntityLayer;
		wallLayerMask = 1 << LayersManager.layersManager.wallLayer;
		raycastLayerMask = wallEntityLayerMask | wallLayerMask;
		
		minPositionX = Mathf.RoundToInt(boundaryWW.transform.position.x);  // ASSUMPTION: will never round out of bounds
		maxPositionX = Mathf.RoundToInt(boundaryEE.transform.position.x);
		minPositionY = Mathf.RoundToInt(boundarySS.transform.position.y);
		maxPositionY = Mathf.RoundToInt(boundaryNN.transform.position.y);
		maxIndexX = maxPositionX - minPositionX;
		maxIndexY = maxPositionY - minPositionY;

		InitializeUnitVectors();
		InitializeEnvironmentGraph();
		circleSmallArray = new GameObject[maxIndexX + 1, maxIndexY + 1];
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
		environmentGraph = new bool[maxIndexX + 1, maxIndexY + 1, unitVectorDirections.Length];
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

	// TODO get neighbors function? 
	
	/**
     * Prints environmentGrid on keypress P
     */
	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			if (circleSmallArray[0,0] == null) {
				spawnCircleGrid();
			} else {
				despawnCircleGrid();
			}
		}
	}

	private void spawnCircleGrid() {
		//for (float positionX = minPositionX; positionX <= maxPositionX; positionX++) {
		//	for (float positionY = minPositionY; positionY <= maxPositionY; positionY++) {
		//		Vector2 position = new Vector2(positionX, positionY);
		for (int indexX = 0; indexX < environmentGraph.GetLength(0); indexX++) {
			for (int indexY = 0; indexY < environmentGraph.GetLength(1); indexY++) {
				Vector2 position = new Vector2(indexX + minPositionX, indexY + minPositionY);
				circleSmallArray[indexX, indexY] = Instantiate(PrefabReferences.prefabReferences.circleSmall2, position, Quaternion.identity);
				if (environmentGraph[indexX, indexY, 0]) {
					circleSmallArray[indexX, indexY].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
				}
			}
		}
	}

	private void despawnCircleGrid() {
		for (int indexX = 0; indexX < environmentGraph.GetLength(0); indexX++) {
			for (int indexY = 0; indexY < environmentGraph.GetLength(1); indexY++) {
				Destroy(circleSmallArray[indexX, indexY]);
			}
		}
	}

	public float getPositionX(int indexX) {
		float positionX = indexX + minPositionX;
		return positionX;
	}

	public float getPositionY(int indexY) {
		float positionY = indexY + minPositionY;
		return positionY;
	}

	public int getIndexX(float positionX) {
		int indexX = Mathf.RoundToInt(positionX) - minPositionX;
		return indexX;
	}

	public int getIndexY(float positionY) {
		int indexY = Mathf.RoundToInt(positionY) - minPositionY;
		return indexY;
	}

	public static float euclideanDistance(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		return Mathf.Sqrt((targetIndexX - indexX) * (targetIndexX - indexX) + (targetIndexY - indexY) * (targetIndexY - indexY));
	}

	/**
     * min 8 directional distance weighting 1 on axis-parallel directions and sqrt(2) on diagonal directions
     */
	public static float manhattanDiagonalDistance(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		int deltaXIndex = Mathf.Abs(targetIndexX - indexX);
		int deltaYIndex = Mathf.Abs(targetIndexY - indexY);
		int deltaDiagonalIndex = Mathf.Min(deltaXIndex, deltaYIndex);
		int deltaAxisParallelIndex = Mathf.Abs(deltaXIndex - deltaYIndex);
		return deltaAxisParallelIndex + Mathf.Sqrt(2) * deltaDiagonalIndex;
	}

	public static float manhattanDistance(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		return Mathf.Abs(targetIndexX - indexX) + Mathf.Abs(targetIndexY - indexY);
	}
}
