using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Priority_Queue;

/**
 * Abstracted behaviors for easy access later. 
 */
public abstract class CompleteBodyController : SpiritController {

	public CompleteBody completeBody;  // set in inspector

	static float epsilon;
	protected static GameObject[,] circleSmallGrid;  // for debugging purposes
	protected Stack<AStarPriorityQueueNode> path;
	protected AStarPriorityQueueNode nextNode;
	protected Vector2 nextNodePosition;  // redundant given this.nextNode

	public float[] personalityUniform;
	public float[] personalityGaussian;

	static CompleteBodyController() {
		epsilon = 0.0001f;
	}

	protected override void Awake() {
		base.Awake();
		completeBody = GetComponent<CompleteBody>();
	}

	protected override void Start() {
		base.Start();
		
		circleSmallGrid = new GameObject[EnvironmentManager.environmentManager.XN, EnvironmentManager.environmentManager.YM];
		path = new Stack<AStarPriorityQueueNode>();
		nextNodePosition = transform.position;
		nextNode = new AStarPriorityQueueNode(0f, EnvironmentManager.environmentManager.GetIndexX(this.nextNodePosition.x), EnvironmentManager.environmentManager.GetIndexY(this.nextNodePosition.y), -1, -1);

		personalityUniform = MyStaticLibrary.NextRandomUniformArray(5, -1f, 1f);
		personalityGaussian = MyStaticLibrary.NextRandomGaussianArray(5);
	}

	protected virtual void Update() {
		ManualDebug();
	}

	protected virtual void FixedUpdate() {
		// alertCooldownTime -= Time.deltaTime;  // float.MinValue ~ -3.4E38 seconds
		Rotate();
		Move();
		Dash();
		Fire();
		Reload();
		HandleItem();
		PocketHandItem();
	}

	protected virtual void ManualDebug() { }

	protected virtual void Rotate() { }

	protected virtual void Move() { }

	protected virtual void Dash() { }

	protected virtual void Fire() { }

	protected virtual void SafeFire(int eeiHand, bool MB, bool MBD) {
		if (completeBody.GetEquipmentEquipableArray()[eeiHand] != null) {
			IActivatable activatable = completeBody.GetEquipmentEquipableArray()[eeiHand] as IActivatable;
			Dictionary<object, object> argumentDictionary = new Dictionary<object, object>();
			argumentDictionary["MB"] = MB;
			argumentDictionary["MBD"] = MBD;
			Activator.Activate(completeBody, activatable, argumentDictionary);
		}
	}

	protected virtual void Reload() { }

	protected virtual void HandleItem() { }

	protected virtual void PocketHandItem() { }

	/**
	 * After updating, we rotate and move towards the nextNodeToTargetPosition
	 */
	protected void UpdateNextNodePosition() {
		while (IsAtNextNodePosition() && this.path.Count > 0) {
			this.nextNode = this.path.Pop();
			this.nextNodePosition.x = EnvironmentManager.environmentManager.GetPositionX(this.nextNode.indexX);
			this.nextNodePosition.y = EnvironmentManager.environmentManager.GetPositionY(this.nextNode.indexY);
		}
	}

	protected bool IsAtNextNodePosition() {
		bool isAtNextNodePosition = MyStaticLibrary.GetDistance(transform.position, nextNodePosition) < MyStaticLibrary._sqrt0_5;
		return isAtNextNodePosition;
	}

	///**
 //    * Updates targetPosition to be the closestHostileAgent position
 //    */
	//protected IEnumerator KeepFindPrimeAdversary() {
	//	while (presentAllyList.Count > 0) {
	//		FindPrimeAdversary();
	//		yield return new WaitForSeconds(1f);
	//	}
	//}

	///**
 //    * Returns closest circle agent who is on a different team or null if none. 
 //    */
	//protected void FindPrimeAdversary() {
	//	// CircleAgent[] circleAgentArray = FindObjectsOfType<CircleAgent>();
	//	primeAdversary = null;
	//	float primeAdversaryTransformDistance = float.MaxValue;
	//	for (int pa = 0; pa < presentAdversaryList.Count; pa++) {
	//		if (presentAdversaryList[pa].affinity != agent.affinity && !presentAdversaryList[pa].defunct) {
	//			float distance = MyStaticLibrary.GetDistance(gameObject, presentAdversaryList[pa].gameObject);
	//			if (distance < primeAdversaryTransformDistance) {
	//				primeAdversary = presentAdversaryList[pa];
	//				primeAdversaryTransformDistance = distance;
	//			}
	//		}
	//	}
	//}

	protected virtual IEnumerator FindPathAStarSearch(Vector2 targetPosition) {
		int frontierSize = EnvironmentManager.environmentManager.XN + EnvironmentManager.environmentManager.YM;  // ASSUMPTION: frontierSize > 0
		FastPriorityQueue<AStarPriorityQueueNode> frontier = new FastPriorityQueue<AStarPriorityQueueNode>(frontierSize);
		AStarPriorityQueueNode[,] nodeGrid = new AStarPriorityQueueNode[EnvironmentManager.environmentManager.XN, EnvironmentManager.environmentManager.YM];  // element is null if never in frontier before

		/**
		 * source node and target node
		 */
		// AStarPriorityQueueNode sourceNode = this.path.Skip(1).FirstOrDefault();  // HACK: assume AStarSearch will finish by the time agent reaches next next node
		int sourceIndexX = EnvironmentManager.environmentManager.GetIndexX(transform.position.x);
		int sourceIndexY = EnvironmentManager.environmentManager.GetIndexY(transform.position.y);
		int targetIndexX = EnvironmentManager.environmentManager.GetIndexX(targetPosition.x);
		int targetIndexY = EnvironmentManager.environmentManager.GetIndexY(targetPosition.y);

		if (!EnvironmentManager.environmentManager.environmentGraph[sourceIndexX, sourceIndexY, 0] || !EnvironmentManager.environmentManager.environmentGraph[targetIndexX, targetIndexY, 0]) {
			Debug.Log("astar source or target in entity wall");  // occurs if target corpse slides through wall
			yield break;
		}

		/**
		 * Explore frontier from source node to target node
		 */
		AStarPriorityQueueNode expandedTargetNode = null;
		float sourceNodePathCost = 0f;
		nodeGrid[sourceIndexX, sourceIndexY] = new AStarPriorityQueueNode(sourceNodePathCost, sourceIndexX, sourceIndexY, -1, -1);
		float sourceNodePriority = nodeGrid[sourceIndexX, sourceIndexY].pastPathCost + futurePathCostHeuristic(sourceIndexX, sourceIndexY, targetIndexX, targetIndexY);
		frontier.Enqueue(nodeGrid[sourceIndexX, sourceIndexY], sourceNodePriority);
		while (frontier.Count > 0) {
			AStarPriorityQueueNode node = frontier.Dequeue();
			yield return InstantiateNodeAStarSearch(node, 0.01f, new Color(1, 1, 1));  // for debugging purposes

			if (node.indexX == targetIndexX && node.indexY == targetIndexY) {  // if path found, then break
				expandedTargetNode = node;
				break;
			}

			/**
			 * Expand the node by adding its neighbors to the frontier
			 */
			int[][] validNeighborIndicesXY = EnvironmentManager.environmentManager.GetValidNeighborIndicesXY(node.indexX, node.indexY);
			for (int vv = 0; vv < validNeighborIndicesXY.Length; vv++) {
				if (validNeighborIndicesXY[vv] != null) {
					int neighborNodeIndexX = validNeighborIndicesXY[vv][0];
					int neighborNodeIndexY = validNeighborIndicesXY[vv][1];

					float neighborNodePathCost = node.pastPathCost + EnvironmentManager.unitVectorMagnitudes[vv + 1];
					if (nodeGrid[neighborNodeIndexX, neighborNodeIndexY] == null) {  // node has no parent
						if (frontier.Count == frontierSize) {
							frontierSize *= 2;
							frontier.Resize(frontierSize);
						}
						nodeGrid[neighborNodeIndexX, neighborNodeIndexY] = new AStarPriorityQueueNode(neighborNodePathCost, neighborNodeIndexX, neighborNodeIndexY, node.indexX, node.indexY);

						float neighborNodePriority = nodeGrid[neighborNodeIndexX, neighborNodeIndexY].pastPathCost + futurePathCostHeuristic(neighborNodeIndexX, neighborNodeIndexY, targetIndexX, targetIndexY);
						frontier.Enqueue(nodeGrid[neighborNodeIndexX, neighborNodeIndexY], neighborNodePriority);
					} else if (nodeGrid[neighborNodeIndexX, neighborNodeIndexY].pastPathCost - neighborNodePathCost > epsilon) {  // node has a worse parent
						nodeGrid[neighborNodeIndexX, neighborNodeIndexY].pastPathCost = neighborNodePathCost;
						nodeGrid[neighborNodeIndexX, neighborNodeIndexY].parentIndexX = node.indexX;
						nodeGrid[neighborNodeIndexX, neighborNodeIndexY].parentIndexY = node.indexY;

						float neighborNodePriority = nodeGrid[neighborNodeIndexX, neighborNodeIndexY].pastPathCost + futurePathCostHeuristic(neighborNodeIndexX, neighborNodeIndexY, targetIndexX, targetIndexY);
						frontier.UpdatePriority(nodeGrid[neighborNodeIndexX, neighborNodeIndexY], neighborNodePriority);
					} 
					// else { }  // this is if the neighbor already has a better parent; note that if a neighbor has already been dequeued from the frontier, then it will have a better parent because its best parent has already been found
				}
			}
		}

		yield return tracePath(nodeGrid, expandedTargetNode);
	}

	private IEnumerator tracePath(AStarPriorityQueueNode[,] nodeGrid, AStarPriorityQueueNode expandedTargetNode) {
		Stack<AStarPriorityQueueNode> path = new Stack<AStarPriorityQueueNode>();
		if (expandedTargetNode != null) {
			AStarPriorityQueueNode nextExpandedNode = expandedTargetNode;
			path.Push(nextExpandedNode);
			yield return InstantiateNodeAStarSearch(nextExpandedNode, 0.1f, new Color(0, 0, 0));  // for debugging purposes
			while (nextExpandedNode.parentIndexX != -1) {
				nextExpandedNode = nodeGrid[nextExpandedNode.parentIndexX, nextExpandedNode.parentIndexY];
				path.Push(nextExpandedNode);
				yield return InstantiateNodeAStarSearch(nextExpandedNode, 0.1f, new Color(0, 0, 0));  // for debugging purposes
			}
		}
		this.path = path;
		yield return null;
	}

	/**
	 * Places a circle node at index location mostly for demo purposes
	 */
	private IEnumerator InstantiateNodeAStarSearch(AStarPriorityQueueNode node, float waitTime, Color color) {
		yield return new WaitForSeconds(waitTime);
		// node.Print();
		Vector2 position = new Vector2(EnvironmentManager.environmentManager.GetPositionX(node.indexX), EnvironmentManager.environmentManager.GetPositionY(node.indexY));  // TODO
		GameObject nodeGameObject = Instantiate(PrefabReferences.prefabReferences.circleSmall2, position, Quaternion.identity);
		nodeGameObject.GetComponent<SpriteRenderer>().color = color;

		if (circleSmallGrid[node.indexX, node.indexY] != null) {
			Destroy(circleSmallGrid[node.indexX, node.indexY]);
		}
		circleSmallGrid[node.indexX, node.indexY] = nodeGameObject;
	}

	protected IEnumerator ErasePathNodes() {
		for (int indexX = 0; indexX < EnvironmentManager.environmentManager.XN; indexX++) {
			for (int indexY = 0; indexY < EnvironmentManager.environmentManager.YM; indexY++) {
				if (circleSmallGrid[indexX, indexY] != null) {
					Destroy(circleSmallGrid[indexX, indexY]);
				}
			}
		}
		yield return null;
	}

	private static float futurePathCostHeuristic(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		return EnvironmentManager.ManhattanDiagonalDistance(indexX, indexY, targetIndexX, targetIndexY);
	}
}
