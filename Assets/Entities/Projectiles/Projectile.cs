﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO: Bullet expires from losing speed
 * TODO: Bullet extends projectile
 * TODO: Bullet damage depends on mass? 
 * TODO: Bullet mass depends on prefab for different ammunition types
 * 
 * TODO: Energy/elemental martial arts bending, in contrast to bullets
 */
public class Projectile : SpriteBody {

	public CompleteBody completeBodyActivator;  //set beforehand by casterAgent
	public float timeout;
	public float initialVelocity;
	public float baseDamage;
	public bool defunct;
	
	protected override void Start() {
		base.Start();
		defunct = false;
	}

	/**
     * Projectiles have a limited range and duration
     */
	protected override void FixedUpdate() {
		base.FixedUpdate();
		if (!defunct) {
			timeout -= Time.fixedDeltaTime;
			if (timeout <= 0) {
				defunct = true;
				Disintegrate();
			}
			//Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
		}
	}

	protected override int GetTeamLayer() {
		return LayersManager.layersManager.GetTeamProjectileLayer(GetAffinity());
	}

	/**
     * Damages enemies upon impact. 
     */
	//protected virtual void OnCollisionEnter2D(Collision2D collision) {
	//	Entity collisionGameObjectEntity = collision.gameObject.GetComponent<Entity>();
	//	Debug.Log("collision enter " + collisionGameObjectEntity);
	//}

	//protected virtual void OnCollisionStay2D(Collision2D collision) {
	//	Entity collisionGameObjectEntity = collision.gameObject.GetComponent<Entity>();
	//	Debug.Log("collision stay " + collisionGameObjectEntity);
	//}

	//protected virtual void OnCollisionExit2D(Collision2D collision) {
	//	Entity collisionGameObjectEntity = collision.gameObject.GetComponent<Entity>();
	//	Debug.Log("collision exit " + collisionGameObjectEntity);
	//}

	protected virtual void OnTriggerEnter2D(Collider2D collider) {
		if (collider.name == "Body" || collider.name == "Head") {
			CompleteBody collisionGameObjectCompleteBody = collider.GetComponentInParent<CompleteBody>();
			//Debug.Log("trigger enter " + collisionGameObjectEntity + " " + collider.name + " velocity ");
			if (collisionGameObjectCompleteBody != completeBodyActivator && collider.name == "Body") {
				GetComponent<Rigidbody2D>().drag += collisionGameObjectCompleteBody.viscosity;
				GetComponent<Rigidbody2D>().angularDrag += collisionGameObjectCompleteBody.viscosity;
			}
		}
	}

	protected virtual void OnTriggerStay2D(Collider2D collider) {
		if (collider.name == "Body" || collider.name == "Head") {
			CompleteBody collisionGameObjectCompleteBody = collider.GetComponentInParent<CompleteBody>();
			// Debug.Log("trigger stay " + collisionGameObjectEntity + " " + collider.name + " velocity ");
			if (collisionGameObjectCompleteBody.GetAffinity() != completeBodyActivator.GetAffinity()) {
				float speedFactor = Mathf.Pow((GetComponent<Rigidbody2D>().velocity - collisionGameObjectCompleteBody.GetComponent<Rigidbody2D>().velocity).magnitude / initialVelocity, 2);
				float damage = baseDamage * speedFactor;
				collisionGameObjectCompleteBody.TakeDamage(completeBodyActivator, damage);
				// Debug.Log(speedFactor);
				// Debug.Log(damage);
			}
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collider) {
		if (collider.name == "Body" || collider.name == "Head") {
			CompleteBody collisionGameObjectCompleteBody = collider.GetComponentInParent<CompleteBody>();
			//Debug.Log("trigger exit " + collisionGameObjectEntity + " " + collider.name);
			if (collisionGameObjectCompleteBody != completeBodyActivator && collider.name == "Body") {
				GetComponent<Rigidbody2D>().drag -= collisionGameObjectCompleteBody.viscosity;
				GetComponent<Rigidbody2D>().angularDrag -= collisionGameObjectCompleteBody.viscosity;
			}
		}
	}
}
