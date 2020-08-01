using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhysicsBody {
	void RotateTargetPosition(Vector2 targetPosition);
	void RotateOffsetRotation(float offsetRotation);
	void MoveTargetPosition(Vector2 targetPosition, bool crawl = false);
	void MoveWASD(bool D, bool A, bool W, bool S, bool crawl = false);
	void DashWASD(bool D, bool A, bool W, bool S);
	float GetRadius();
}
