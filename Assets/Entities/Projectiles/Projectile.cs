using System;
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
public class Projectile : Entity {

	public Body casterAgent;  //set beforehand by casterAgent
	public float timeout;
	public float initialVelocity;
	public float baseDamage;
	
	protected override void Start() {
		// base.Start();
		//GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(0, initialVelocity));//too slow to go and causes inaccurate shooting; moved to gun as opposed to bullet
	}

	public virtual void LateStart() {
		base.Start();
	}

	/**
     * Projectiles have a limited range and duration
     */
	protected override void FixedUpdate() {
		base.FixedUpdate();
		timeout -= Time.fixedDeltaTime;
		if (timeout <= 0) {
			Disintegrate();
		}
		//Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
	}

	protected override int GetTeamLayer() {
		return LayersManager.layersManager.GetTeamProjectileLayer(affinity);
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
			Body collisionGameObjectEntity = collider.GetComponentInParent<Body>();
			//Debug.Log("trigger enter " + collisionGameObjectEntity + " " + collider.name + " velocity ");
			if (collisionGameObjectEntity != casterAgent && collider.name == "Body") {
				GetComponent<Rigidbody2D>().drag += collisionGameObjectEntity.viscosity;
				GetComponent<Rigidbody2D>().angularDrag += collisionGameObjectEntity.viscosity;
			}
		}
	}

	protected virtual void OnTriggerStay2D(Collider2D collider) {
		if (collider.name == "Body" || collider.name == "Head") {
			Body collisionGameObjectEntity = collider.GetComponentInParent<Body>();
			// Debug.Log("trigger stay " + collisionGameObjectEntity + " " + collider.name + " velocity ");
			if (collisionGameObjectEntity.affinity != casterAgent.affinity) {
				float speedFactor = Mathf.Pow((GetComponent<Rigidbody2D>().velocity - collisionGameObjectEntity.GetComponent<Rigidbody2D>().velocity).magnitude / initialVelocity, 2);
				float damage = baseDamage * speedFactor;
				collisionGameObjectEntity.takeDamage(casterAgent, damage);
				//Debug.Log(speedFactor);
				Debug.Log(damage);
			}
		}
	}

	protected virtual void OnTriggerExit2D(Collider2D collider) {
		if (collider.name == "Body" || collider.name == "Head") {
			Body collisionGameObjectEntity = collider.GetComponentInParent<Body>();
			//Debug.Log("trigger exit " + collisionGameObjectEntity + " " + collider.name);
			if (collisionGameObjectEntity != casterAgent && collider.name == "Body") {
				GetComponent<Rigidbody2D>().drag -= collisionGameObjectEntity.viscosity;
				GetComponent<Rigidbody2D>().angularDrag -= collisionGameObjectEntity.viscosity;
			}
		}
	}
}
