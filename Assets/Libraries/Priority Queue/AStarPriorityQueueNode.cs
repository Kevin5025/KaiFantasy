using System.Collections;

namespace Priority_Queue {
	public class AStarPriorityQueueNode : FastPriorityQueueNode {

		public int indexX;
		public int indexY;
		public float pastPathCost;

		public AStarPriorityQueueNode(int indexX, int indexY, float pathCost) {
			this.indexX = indexX;
			this.indexY = indexY;
			this.pastPathCost = pathCost;
		}

	}
}
