using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M9 : ActivatableEquipable {

	public M9(EquipableClass equipableClass) : base(equipableClass, 0.4f) {
		
	}

	public override void Actuate(CircleAgent casterAgent) {
		//Debug.Log("M9");
		Vector2 headPosition = casterAgent.transform.TransformPoint(new Vector2(0, 0.6f * casterAgent.radius));
		GameObject projectileGameObject = GameObject.Instantiate(PrefabReferences.prefabReferences.bullet, headPosition, casterAgent.transform.rotation);

		Bullet projectile = projectileGameObject.AddComponent<Bullet>();
		projectile.affinity = casterAgent.affinity;
		projectile.casterAgent = casterAgent;
		projectile.timeout = 1.0f;
		projectile.initialVelocity = 20f;
		projectile.baseDamage = 10f;

		projectileGameObject.GetComponent<Rigidbody2D>().velocity = projectileGameObject.transform.TransformDirection(new Vector2(0, projectile.initialVelocity));
	}

}
