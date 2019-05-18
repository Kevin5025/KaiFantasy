using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M9 : ActivatableEquipable {

	public M9(EquipableClass equipableClass) : base(equipableClass, 1f) {
		
	}

	public override void Actuate(CircleAgent casterAgent) {
		Debug.Log("M9");
		Vector2 headPosition = casterAgent.transform.TransformPoint(new Vector2(0, 0.0f * casterAgent.radius));//TODO: 0.6
		GameObject projectileGameObject = GameObject.Instantiate(PrefabReferences.prefabReferences.circleSmall2, headPosition, casterAgent.transform.rotation);

		Bullet projectile = projectileGameObject.AddComponent<Bullet>();
		projectile.affinity = casterAgent.affinity;
		projectile.casterAgent = casterAgent;
		projectile.timeout = 1.0f;
		projectile.initialVelocity = 20f;
		projectile.baseDamage = 40f;
	}

}
