using UnityEngine;
using System.Collections;

namespace Priority_Queue {
	public class AStarPriorityQueueNode : FastPriorityQueueNode {

		public int indexX;
		public int indexY;
		public float pastPathCost;
		public int parentIndexX;
		public int parentIndexY;

		public AStarPriorityQueueNode(float pastPathCost, int indexX, int indexY, int parentIndexX, int parentIndexY) {
			this.pastPathCost = pastPathCost;
			this.indexX = indexX;
			this.indexY = indexY;
			this.parentIndexX = parentIndexX;
			this.parentIndexY = parentIndexY;
		}

		public void Print() {
			Debug.Log("frontier node: " + this.indexX + ", " + this.indexY + ", " + this.pastPathCost + ", " + this.Priority);
		}
	}
}
