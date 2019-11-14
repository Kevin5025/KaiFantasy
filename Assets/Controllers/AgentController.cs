using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

/**
 * Abstracted behaviors for easy access later. 
 */
public abstract class AgentController : MonoBehaviour {

	protected CircleAgent agent;

	// TODO: nest into map that takes "ally" and "adversary" as keys
	protected IList<CircleAgent> presentAllyList;
	protected IList<CircleAgent> presentAdversaryList;
	public CircleAgent primeAlly;  // TODO: antagonist / protagonist? 
	public CircleAgent primeAdversary;

	public float[] personalityUniform;
	public float[] personalityGaussian;

	protected virtual void Awake() { }

	protected virtual void Start() {
		agent = GetComponent<CircleAgent>();
		presentAllyList = new List<CircleAgent>();
		presentAdversaryList = new List<CircleAgent>();
		primeAlly = null;
		primeAdversary = null;

		personalityUniform = MyStaticLibrary.NextRandomUniformArray(5, -1f, 1f);
		personalityGaussian = MyStaticLibrary.NextRandomGaussianArray(5);
	}

	protected virtual void Update() { }

	protected virtual void FixedUpdate() {
		// alertCooldownTime -= Time.deltaTime;  // float.MinValue ~ -3.4E38 seconds
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider) {
		CircleAgent colliderCircleAgent = collider.GetComponent<CircleAgent>();
		if (colliderCircleAgent != null) {
			if (colliderCircleAgent.affinity == agent.affinity) {
				presentAllyList.Add(colliderCircleAgent);
				// Debug.Log(presentAllyList.Count);
				if (primeAlly == null) {
					FindPrimeAlly();
				}
			} else {
				presentAdversaryList.Add(colliderCircleAgent);
				// Debug.Log(presentAdversaryList.Count);
				if (primeAdversary == null) {
					FindPrimeAdversary();
				}
			}
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collider) {
		CircleAgent colliderCircleAgent = collider.GetComponent<CircleAgent>();
		if (colliderCircleAgent != null) {
			if (colliderCircleAgent.affinity == agent.affinity) {
				presentAllyList.Remove(colliderCircleAgent);
				// Debug.Log(presentAllyList.Count);
				if (colliderCircleAgent == primeAlly) {
					FindPrimeAlly();
				}
			} else {
				presentAdversaryList.Remove(colliderCircleAgent);
				// Debug.Log(presentAdversaryList.Count);
				if (colliderCircleAgent == primeAdversary) {
					FindPrimeAdversary();
				}
			}
		}
	}

	protected virtual void Rotate() { }

	protected virtual void Move() { }

	protected virtual void Fire() { }

	/**
     * Returns prime present ally circle agent or null if none. 
     */
	protected virtual void FindPrimeAlly() {
		if (presentAllyList.Count > 0) {
			primeAlly = presentAllyList[0];
		} else {
			primeAlly = null;
		}
	}

	/**
     * Returns prime present adversary circle agent or null if none. 
     */
	protected virtual void FindPrimeAdversary() {
		if (presentAdversaryList.Count > 0) {
			primeAdversary = presentAdversaryList[0];
		} else {
			primeAdversary = null;
		}
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
		bool[,,] environmentGraph = EnvironmentManager.environmentManager.environmentGraph;
		int XN = environmentGraph.GetLength(0);
		int YM = environmentGraph.GetLength(1);
		int[,] parentIndexXGrid = new int[XN, YM];
		int[,] parentIndexYGrid = new int[XN, YM];
		int frontierSize = XN + YM;  // ASSUMPTION: frontierSize > 0
		FastPriorityQueue<AStarPriorityQueueNode> frontier = new FastPriorityQueue<AStarPriorityQueueNode>(frontierSize);  // TODO: logic to resize when necessary
		AStarPriorityQueueNode[,] nodeGrid = new AStarPriorityQueueNode[XN, YM];  // null if not visited

		/**
		 * source node and target node
		 */
		int sourceIndexX = EnvironmentManager.environmentManager.getIndexX(transform.position.x);
		int sourceIndexY = EnvironmentManager.environmentManager.getIndexY(transform.position.y);
		int targetIndexX = EnvironmentManager.environmentManager.getIndexX(targetPosition.x);
		int targetIndexY = EnvironmentManager.environmentManager.getIndexY(targetPosition.y);
		float startPathCost = 0f;

		if (!environmentGraph[sourceIndexX, sourceIndexY, 0] || !environmentGraph[targetIndexX, targetIndexY, 0]) {
			Debug.Log("astar source or target in entity wall");  // occurs if target corpse slides through wall
			yield break;
		}

		/**
		 * Explore frontier from source node to target node
		 */
		nodeGrid[sourceIndexX, sourceIndexY] = new AStarPriorityQueueNode(sourceIndexX, sourceIndexY, startPathCost);
		float sourceNodePriority = nodeGrid[sourceIndexX, sourceIndexY].pastPathCost + futureDistanceHeuristic(sourceIndexX, sourceIndexY, targetIndexX, targetIndexX);
		frontier.Enqueue(nodeGrid[sourceIndexX, sourceIndexY], sourceNodePriority);
		parentIndexXGrid[sourceIndexX, sourceIndexY] = -1;
		parentIndexYGrid[sourceIndexX, sourceIndexY] = -1;
		while (frontier.Count > 0) {
			AStarPriorityQueueNode node = frontier.Dequeue();
			if (node.indexX == targetIndexX && node.indexY == targetIndexY) {  // if path found
				break;
			}

			/**
			 * Expand the node by adding its neighbors to the frontier
			 */
			// 
		}
	}

	private float futureDistanceHeuristic(int indexX, int indexY, int targetIndexX, int targetIndexY) {
		return EnvironmentManager.manhattanDiagonalDistance(indexX, indexY, targetIndexX, targetIndexY);
	}
}
