using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

	public static EnvironmentManager environmentManager_;

	protected int wallEntityLayerMask;
	protected int wallLayerMask;
	protected int raycastLayerMask;

	public GameObject boundaryNN_;
	public GameObject boundaryEE_;
	public GameObject boundarySS_;
	public GameObject boundaryWW_;

	public static Vector2 unitVectorNorthEast;
	public static Vector2 unitVectorSouthEast;
	public static Vector2 unitVectorSouthWest;
	public static Vector2 unitVectorNorthWest;
	public static Vector2[] unitVectorDirections;
	public static float[] unitVectorMagnitudes;
	public static int[] unitVectorDirectionsX;
	public static int[] unitVectorDirectionsY;

	public int minPositionX;
	public int maxPositionX;
	public int minPositionY;
	public int maxPositionY;
	public int maxIndexX;
	public int maxIndexY;

	public bool[,,] environmentGraph;  // returns whether (X, Y, V) is a valid vector direction, the V=0 direction tells us whether the position is clear
	public int XN;
	public int YM;
	private GameObject[,] circleSmallArray;

	static EnvironmentManager() {
		InitializeUnitVectors();
	}

	void Awake() {
		if (environmentManager_ == null) {
			environmentManager_ = this;
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		wallEntityLayerMask = 1 << LayersManager.layersManager.wallEntityLayer;
		wallLayerMask = 1 << LayersManager.layersManager.wallLayer;
		raycastLayerMask = wallEntityLayerMask | wallLayerMask;
		
		minPositionX = Mathf.RoundToInt(boundaryWW_.transform.position.x);  // ASSUMPTION: will never round out of bounds
		maxPositionX = Mathf.RoundToInt(boundaryEE_.transform.position.x);
		minPositionY = Mathf.RoundToInt(boundarySS_.transform.position.y);
		maxPositionY = Mathf.RoundToInt(boundaryNN_.transform.position.y);
		maxIndexX = maxPositionX - minPositionX;
		maxIndexY = maxPositionY - minPositionY;

		InitializeEnvironmentGraph();
		XN = environmentGraph.GetLength(0);
		YM = environmentGraph.GetLength(1);
		circleSmallArray = new GameObject[maxIndexX + 1, maxIndexY + 1];
	}

	protected static void InitializeUnitVectors() {  // TODO: should be in a static constructor
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

		unitVectorDirectionsX = new int[] { 0, 0, 1, 0, -1, 1, 1, -1, -1 };
		unitVectorDirectionsY = new int[] { 0, 1, 0, -1, 0, 1, -1, -1, 1 };
	}

	protected void InitializeEnvironmentGraph() {
		environmentGraph = new bool[maxIndexX + 1, maxIndexY + 1, unitVectorDirections.Length];
		for (int indexX = 0; indexX < environmentGraph.GetLength(0); indexX++) {
			for (int indexY = 0; indexY < environmentGraph.GetLength(1); indexY++) {
				Vector2 position = new Vector2(indexX + minPositionX, indexY + minPositionY);
				for (int v = 0; v < environmentGraph.GetLength(2); v++) {
					RaycastHit2D raycastHitDirection = Physics2D.Raycast(position, unitVectorDirections[v], unitVectorMagnitudes[v], raycastLayerMask, 0f, 0f);
					environmentGraph[indexX, indexY, v] = raycastHitDirection.collider == null;
				}
			}
		}
	}

	public int[][] GetValidNeighborIndicesXY(int indexX, int indexY) {
		int[][] validNeighborIndicesXY = new int[unitVectorDirections.Length-1][];
		for (int v = 1; v < unitVectorDirections.Length; v++) {  // ignore v = 0
			if (environmentGraph[indexX, indexY, v]) {
				validNeighborIndicesXY[v-1] = new int[] { indexX + unitVectorDirectionsX[v], indexY + unitVectorDirectionsY[v] };  // ASSUMPTION: there are walls preventing index out of bounds
			}  // else null
		}
		return validNeighborIndicesXY;
	}
	
	/**
     * Prints environmentGrid on keypress P
     */
	void Update() {
		if (Input.GetKeyDown(KeyCode.P)) {
			if (circleSmallArray[0,0] == null) {
				SpawnCircleGrid();
			} else {
				DespawnCircleGrid();
			}
		}
	}

	private void SpawnCircleGrid() {
		//for (float positionX = minPositionX; positionX <= maxPositionX; positionX++) {
		//	for (float positionY = minPositionY; positionY <= maxPositionY; positionY++) {
		//		Vector2 position = new Vector2(positionX, positionY);
		for (int indexX = 0; indexX < environmentGraph.GetLength(0); indexX++) {
			for (int indexY = 0; indexY < environmentGraph.GetLength(1); indexY++) {
				Vector2 position = new Vector2(indexX + minPositionX, indexY + minPositionY);
				circleSmallArray[indexX, indexY] = Instantiate(PrefabReferences.prefabReferences_.circleSmall2_, position, Quaternion.identity);
				if (!environmentGraph[indexX, indexY, 0]) {
					circleSmallArray[indexX, indexY].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
				}
			}
		}
	}

	private void DespawnCircleGrid() {
		for (int indexX = 0; indexX < environmentGraph.GetLength(0); indexX++) {
			for (int indexY = 0; indexY < environmentGraph.GetLength(1); indexY++) {
				Destroy(circleSmallArray[indexX, indexY]);
			}
		}
	}

	public float GetPositionX(int indexX) {
		float positionX = indexX + minPositionX;
		return positionX;
	}

	public float GetPositionY(int indexY) {
		float positionY = indexY + minPositionY;
		return positionY;
	}

	public int GetIndexX(float positionX) {
		int indexX = Mathf.RoundToInt(positionX) - minPositionX;
		return indexX;
	}

	public int GetIndexY(float positionY) {
		int indexY = Mathf.RoundToInt(positionY) - minPositionY;
		return indexY;
	}

	public static float EuclideanDistance(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		return Mathf.Sqrt((targetIndexX - indexX) * (targetIndexX - indexX) + (targetIndexY - indexY) * (targetIndexY - indexY));
	}

	/**
     * min 8 directional distance weighting 1 on axis-parallel directions and sqrt(2) on diagonal directions
     */
	public static float ManhattanDiagonalDistance(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		int deltaXIndex = Mathf.Abs(targetIndexX - indexX);
		int deltaYIndex = Mathf.Abs(targetIndexY - indexY);
		int deltaDiagonalIndex = Mathf.Min(deltaXIndex, deltaYIndex);
		int deltaAxisParallelIndex = Mathf.Abs(deltaXIndex - deltaYIndex);
		return deltaAxisParallelIndex + Mathf.Sqrt(2) * deltaDiagonalIndex;
	}

	public static float ManhattanDistance(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		return Mathf.Abs(targetIndexX - indexX) + Mathf.Abs(targetIndexY - indexY);
	}
}
