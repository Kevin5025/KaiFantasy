using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OT38 : Item {

	protected override void Start() {
		equipableClass = EquipableClass.HandItem;
		cooldownTimeout = 0.5f;
	}

	public override void Actuate(CircleAgent casterAgent) {
		//Debug.Log("M9");
		Vector2 headPosition = casterAgent.transform.TransformPoint(new Vector2(0, 0.6f * casterAgent.radius));  // 0.297
		GameObject projectileGameObject = GameObject.Instantiate(PrefabReferences.prefabReferences.bullet, headPosition, casterAgent.transform.rotation);

		Bullet projectile = projectileGameObject.AddComponent<Bullet>();
		projectile.affinity = casterAgent.affinity;
		projectile.casterAgent = casterAgent;
		projectile.timeout = 1.0f;
		projectile.initialVelocity = 20f;
		projectile.baseDamage = 30f;

		projectileGameObject.GetComponent<Rigidbody2D>().velocity = projectileGameObject.transform.TransformDirection(new Vector2(0, projectile.initialVelocity));
		projectileGameObject.GetComponent<Collider2D>().enabled = true;
	}

}
