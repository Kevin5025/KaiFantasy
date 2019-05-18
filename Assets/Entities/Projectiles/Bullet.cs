using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO: Bullet expires from losing speed
 * TODO: Bullet loses speed as a function of drag and piercings. 
 * TODO: Bullet extends projectile
 * TODO: Contacting body causes bullet to deal damage and lose speed
 * TODO: Contacting head causes bullet to deal more damage
 * TODO: Bullet damage depends on mass and speed loss? 
 * TODO: Bullet mass depends on prefab for different ammunition types
 * TODO: Color trail behind bullet based on bullet speed and team color? 
 * 
 * TODO: Energy/elemental martial arts bending, in contrast to bullets
 */
public class Bullet : Spirit {//TODO: abstract Bullet and various ammo types

	public CircleAgent casterAgent;//set by casterAgent
	public float timeout;
	public float initialVelocity;
	public float baseDamage;
	
	protected override void Start() {
		base.Start();

		Vector3 forwardDirection = transform.TransformDirection(new Vector2(0, 1f));
		GetComponent<Rigidbody2D>().velocity = forwardDirection * 20f;
	}

	/**
     * Projectiles have a limited range and duration
     */
	protected override void FixedUpdate() {
		base.FixedUpdate();
		timeout -= Time.fixedDeltaTime;
		if (timeout <= 0) {
			Expire();
		}
		Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);
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
		//TODO: is there a more elegant way? 
		if (collider.name == "Body" || collider.name == "Head") {
			Entity collisionGameObjectEntity = collider.GetComponentInParent<Entity>();
			Debug.Log("trigger enter " + collisionGameObjectEntity + " " + collider.name + " velocity ");

			if (collisionGameObjectEntity != casterAgent && collider.name == "Body") {
				float newVelocityMagnitude = Math.Max(0, GetComponent<Rigidbody2D>().velocity.magnitude - collisionGameObjectEntity.viscosity);
				GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(0, newVelocityMagnitude));
			}

			if (collisionGameObjectEntity.affinity != casterAgent.affinity) {
				float damage = baseDamage * GetComponent<Rigidbody2D>().velocity.magnitude / initialVelocity;
				collisionGameObjectEntity.takeDamage(casterAgent, damage);
				Debug.Log(damage);
			}
		}
	}

	//protected virtual void OnTriggerStay2D(Collider2D collider) {
	//	if (collider.name == "Body" || collider.name == "Head") {
	//		Entity collisionGameObjectEntity = collider.GetComponentInParent<Entity>();
	//		//Debug.Log("trigger exit " + collisionGameObjectEntity + " " + collider.name);

	//		if (collisionGameObjectEntity != casterAgent && collider.name == "Body") {
	//			float newVelocityMagnitude = Math.Max(0, GetComponent<Rigidbody2D>().velocity.magnitude - collisionGameObjectEntity.viscosity);
	//			GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(0, newVelocityMagnitude));
	//		}
	//	}
	//}

	protected virtual void OnTriggerExit2D(Collider2D collider) {
		if (collider.name == "Body" || collider.name == "Head") {
			Entity collisionGameObjectEntity = collider.GetComponentInParent<Entity>();
			//Debug.Log("trigger exit " + collisionGameObjectEntity + " " + collider.name);

			if (collisionGameObjectEntity != casterAgent && collider.name == "Body") {
				float newVelocityMagnitude = Math.Max(0, GetComponent<Rigidbody2D>().velocity.magnitude - collisionGameObjectEntity.viscosity);
				GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(0, newVelocityMagnitude));
			}
		}
	}
}
