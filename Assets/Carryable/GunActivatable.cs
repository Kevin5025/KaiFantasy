using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunActivatable : Activatable {

	public float projectileTimeout;  // set in inspector
	public float initialVelocity;  // set in inspector
	public float baseDamage;  // set in inspector

	public override void Actuate(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		CompleteBody completeBodyActivator = (CompleteBody)activator;

		Vector2 headPosition = completeBodyActivator.transform.TransformPoint(completeBodyActivator.headPosition);

		PrefabReferences.prefabReferences.bullet.SetActive(false);  // https://answers.unity.com/questions/636079/assign-exposed-vars-before-instantianting-prefab.html
		GameObject projectileGameObject = Instantiate(PrefabReferences.prefabReferences.bullet, headPosition, completeBodyActivator.transform.rotation);

		ISpirit spirit = projectileGameObject.GetComponent<Spirit>();
		spirit.SetAffinity(completeBodyActivator.GetAffinity());
		Projectile projectile = projectileGameObject.GetComponent<Projectile>();
		projectile.completeBodyActivator = completeBodyActivator;
		projectile.timeout = projectileTimeout;
		projectile.initialVelocity = initialVelocity;
		projectile.baseDamage = baseDamage;

		projectileGameObject.SetActive(true);
		projectileGameObject.GetComponent<Rigidbody2D>().velocity = projectileGameObject.transform.TransformDirection(new Vector2(0, projectile.initialVelocity));
		projectileGameObject.GetComponent<Collider2D>().enabled = true;
	}
}
