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
	public float moveForce;
	public float crawlForce;
	public float torque;

	protected override void Start() {
		base.Start();
		rb2D = GetComponent<Rigidbody2D>();
		radius = Mathf.Sqrt(2 * rb2D.inertia / rb2D.mass);
		area = (float)Math.PI * radius * radius;
		GetComponent<Rigidbody2D>().mass = area;
		moveForce = GetComponent<Rigidbody2D>().mass * 25f;
		crawlForce = moveForce * 0.2f;
		torque = GetComponent<Rigidbody2D>().inertia * 50f;

		headPosition = new Vector2(0, 0.6f * radius);  // 0.297
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
		MoveWASD(W, S, D, A);
	}

	/**
     * Moves up for W, down for S, right for D, and left for A. 
     * Diagonal movement for orthogonal combinations of WASD. 
     */
	public virtual void MoveWASD(bool W, bool S, bool D, bool A) {
		if ((int)healthState >= 2) {
			float verticalDirection = 0;
			verticalDirection += W ? 1 : 0;
			verticalDirection += S ? -1 : 0;

			float horizontalDirection = 0;
			horizontalDirection += D ? 1 : 0;
			horizontalDirection += A ? -1 : 0;

			float verticalForce = 0;
			float horizontalForce = 0;
			if ((new Vector2(horizontalDirection, verticalDirection).magnitude > 0)) {
				double direction = Math.Atan2(verticalDirection, horizontalDirection);
				float force = healthState == HealthState.Fibrillating ? crawlForce : moveForce;
				verticalForce = force * (float)Math.Sin(direction);
				horizontalForce = force * (float)Math.Cos(direction);
			}
			//Debug.Log(new Vector2(horizontalForce, verticalForce));
			//if (relative) {
			//    rb2D.AddRelativeForce(power * new Vector2(horizontalForce, verticalForce));
			//} else {
			rb2D.AddForce(new Vector2(horizontalForce, verticalForce));
			//}
		}
	}
}