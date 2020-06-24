using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a special entity that is a circle. 
 * Circles have a certain amount of force and torque in this Universe to move and rotate. 
 */
public class CircleBody : Body {

	protected Rigidbody2D rb2D;
	public float radius;
	public float area;
	public float torque;
	public float moveForce;
	public float crawlForce;
	public float dashImpulse;

	protected GameObject dashGameObject;
	protected Dash dash;

	protected override void Start() {
		base.Start();
		headPosition = new Vector2(0, 0.6f * radius);  // 0.297

		rb2D = GetComponent<Rigidbody2D>();
		radius = Mathf.Sqrt(2 * rb2D.inertia / rb2D.mass);
		area = (float)Math.PI * radius * radius;
		GetComponent<Rigidbody2D>().mass = area;
		torque = GetComponent<Rigidbody2D>().inertia * 50f;
		moveForce = GetComponent<Rigidbody2D>().mass * 25f;
		crawlForce = moveForce * 0.2f;
		dashImpulse = moveForce * 0.5f;

		dashGameObject = Instantiate(PrefabReferences.prefabReferences.dashGameObject);
		dash = dashGameObject.GetComponent<Dash>();
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

	/**
     * Smart rotation based. Uses current angular momentum for predicted rotation and compares to desired rotation. 
     */
	public virtual void RotateTargetPosition(Vector2 targetPosition) {
		if ((int)healthState >= 2) {
			float currentRotation = transform.eulerAngles.z;
			float targetRotation = Mathf.Atan2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y) * -Mathf.Rad2Deg;//0 to 180, then -180 to 0 counterclockwise
			float offsetRotation = targetRotation - currentRotation;
			transform.Rotate(0, 0, offsetRotation);
		}
	}

	public virtual void RotateOffsetRotation(float offsetRotation) {
		if ((int)healthState >= 2) {
			transform.Rotate(0, 0, offsetRotation);
		}
	}

	/**
     * Decides which of the 8 WASD directions (including diagonals) to move, in order to reach target position. 
     */
	public virtual void MoveTargetPosition(Vector2 targetPosition) {
		Vector2 currentPosition = transform.position;
		Vector2 offsetPosition = targetPosition - currentPosition;

		bool W = Math.Atan2(offsetPosition.y, offsetPosition.x) > 1 * Math.PI / 8 && Math.Atan2(offsetPosition.y, offsetPosition.x) < 7 * Math.PI / 8;//offsetPosition.y > 0;
		bool S = Math.Atan2(offsetPosition.y, offsetPosition.x) > -7 * Math.PI / 8 && Math.Atan2(offsetPosition.y, offsetPosition.x) < -1 * Math.PI / 8;//offsetPosition.y < 0;
		bool D = Math.Atan2(offsetPosition.y, offsetPosition.x) > -3 * Math.PI / 8 && Math.Atan2(offsetPosition.y, offsetPosition.x) < 3 * Math.PI / 8;//offsetPosition.x > 0;
		bool A = Math.Atan2(offsetPosition.y, offsetPosition.x) > 5 * Math.PI / 8 || Math.Atan2(offsetPosition.y, offsetPosition.x) < -5 * Math.PI / 8;//offsetPosition.x < 0;
		MoveWASD(D, A, W, S);
	}

	/**
     * Moves up for W, down for S, right for D, and left for A. 
     * Diagonal movement for orthogonal combinations of WASD. 
     */
	public virtual void MoveWASD(bool D, bool A, bool W, bool S) {
		if ((int)healthState >= (int)HealthState.Fibrillating) {
			Vector2 unitVector = PlayerCircleBodyController.GetUnitVector(D, A, W, S);
			float force = healthState == HealthState.Fibrillating ? crawlForce : moveForce;
			Vector2 forceVector = force * unitVector;

			//if (relative) {  // different control scheme
			//    rb2D.AddRelativeForce(forceVector);
			//} else {
			rb2D.AddForce(forceVector);
			//}
		}
	}

	public virtual void DashWASD(bool D, bool A, bool W, bool S) {
		if ((int)healthState >= (int)HealthState.Capable) {
			Dictionary<object, object> argumentDictionary = new Dictionary<object, object>();
			argumentDictionary['D'] = D;
			argumentDictionary['A'] = A;
			argumentDictionary['W'] = W;
			argumentDictionary['S'] = S;
			dash.Activate(this, argumentDictionary);
		}
	}
}