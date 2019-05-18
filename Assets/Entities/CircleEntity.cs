using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a special entity that is a circle. 
 * Circles have a certain amount of force and torque in this Universe to move and rotate. 
 */
public class CircleEntity : Entity {

	protected Rigidbody2D rb2D;
	public float radius;
	public float area;
	public float force;
	public float torque;

	protected override void Start () {
		base.Start();
		rb2D = GetComponent<Rigidbody2D>();
		radius = Mathf.Sqrt(2 * rb2D.inertia / rb2D.mass);
		area = (float) Math.PI * radius * radius;
		GetComponent<Rigidbody2D>().mass = area;
		force = GetComponent<Rigidbody2D>().mass * 25f;
		torque = GetComponent<Rigidbody2D>().inertia * 50f;

		//TODO: More varied stats
		maxHealth = area * 300f;
		health = maxHealth;
		healthRegenerationRate = 0.01f;
		viscosity = 2.5f * (float) Math.PI * radius;//5 * area / diameter
	}

	protected override void FixedUpdate () {
		base.FixedUpdate();
	}

	/**
     * Smart rotation based. Uses current angular momentum for predicted rotation and compares to desired rotation. 
     */
	public virtual void Rotate (Vector2 targetPosition, float power = 1f) {
		if (!defunct) {
			float currentRotation = transform.eulerAngles.z;
			float targetRotation = Mathf.Atan2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y) * -Mathf.Rad2Deg;//0 to 180, then -180 to 0 counterclockwise
			float offsetRotation = targetRotation - currentRotation;
			transform.Rotate(0, 0, offsetRotation);
		}
	}

	/**
     * Decides which of the 8 WASD directions (including diagonals) to move, in order to reach target position. 
     */
	public virtual void Move (Vector2 targetPosition) {
		Vector2 currentPosition = transform.position;
		Vector2 offsetPosition = targetPosition - currentPosition;

		bool W = Math.Atan2(offsetPosition.y, offsetPosition.x) > 1 * Math.PI / 8 && Math.Atan2(offsetPosition.y, offsetPosition.x) < 7 * Math.PI / 8;//offsetPosition.y > 0;
		bool S = Math.Atan2(offsetPosition.y, offsetPosition.x) > -7 * Math.PI / 8 && Math.Atan2(offsetPosition.y, offsetPosition.x) < -1 * Math.PI / 8;//offsetPosition.y < 0;
		bool D = Math.Atan2(offsetPosition.y, offsetPosition.x) > -3 * Math.PI / 8 && Math.Atan2(offsetPosition.y, offsetPosition.x) < 3 * Math.PI / 8;//offsetPosition.x > 0;
		bool A = Math.Atan2(offsetPosition.y, offsetPosition.x) > 5 * Math.PI / 8 || Math.Atan2(offsetPosition.y, offsetPosition.x) < -5 * Math.PI / 8;//offsetPosition.x < 0;
		Move(W, S, D, A);
	}

	/**
     * Moves up for W, down for S, right for D, and left for A. 
     * Diagonal movement for orthogonal combinations of WASD. 
     */
	public virtual void Move (bool W, bool S, bool D, bool A) {
		if (!defunct) {
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
