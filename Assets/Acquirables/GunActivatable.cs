using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunActivatable : Activatable {

	public float projectileTimeout_;
	public float initialSpeed_;
	public float baseDamage_;

	public override void Actuate(IActivator activator, Dictionary<object, object> argumentDictionary = null) {
		CompositeBody compositeBody = activator.GetComponent<CompositeBody>();

		Vector2 headPosition = compositeBody.transform.TransformPoint(compositeBody.headPosition);

		// PrefabReferences.prefabReferences_.bulletPrefab_.SetActive(false);  // done in inspector/editor now  // https://answers.unity.com/questions/636079/assign-exposed-vars-before-instantianting-prefab.html
		GameObject projectileGameObject = Instantiate(PrefabReferences.prefabReferences_.bulletPrefab_, headPosition, compositeBody.transform.rotation, SceneReferences.sceneReferences_.projectilesGameObject_.transform);

		ISpirit spirit = projectileGameObject.GetComponent<Spirit>();
		spirit.SetAffinity(compositeBody.GetAffinity());
		Projectile projectile = projectileGameObject.GetComponent<Projectile>();
		projectile.compositeBody = compositeBody;
		projectile.timeout = projectileTimeout_;
		projectile.initialSpeed = initialSpeed_;
		projectile.baseDamage = baseDamage_;

		projectileGameObject.SetActive(true);
		projectileGameObject.GetComponent<Rigidbody2D>().velocity = projectileGameObject.transform.TransformDirection(new Vector2(0, projectile.initialSpeed));
		projectileGameObject.GetComponent<Collider2D>().enabled = true;
	}
}
