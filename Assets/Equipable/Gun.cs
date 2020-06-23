using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item {

	public float projectileTimeout;  // set in inspector
	public float initialVelocity;  // set in inspector
	public float baseDamage;  // set in inspector

	protected override void Start() {
		base.Start();
		equipableClass = EquipableClass.HandItem;
	}

	public override void Actuate(Body casterAgent, Dictionary<object, object> argumentDictionary = null) {
		base.Actuate(casterAgent);
		Vector2 headPosition = casterAgent.transform.TransformPoint(casterAgent.headPosition);
		GameObject projectileGameObject = GameObject.Instantiate(PrefabReferences.prefabReferences.bullet, headPosition, casterAgent.transform.rotation);

		Projectile projectile = projectileGameObject.AddComponent<Projectile>();
		projectile.affinity = casterAgent.affinity;
		projectile.casterAgent = casterAgent;
		projectile.timeout = projectileTimeout;
		projectile.initialVelocity = initialVelocity;
		projectile.baseDamage = baseDamage;
		projectile.LateStart();

		projectileGameObject.GetComponent<Rigidbody2D>().velocity = projectileGameObject.transform.TransformDirection(new Vector2(0, projectile.initialVelocity));
		projectileGameObject.GetComponent<Collider2D>().enabled = true;
	}
}
